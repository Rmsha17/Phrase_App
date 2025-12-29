using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.Models;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly PhraseDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, IEmailService emailService, PhraseDbContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _emailService = emailService;
        _context = context;
        _configuration = configuration;
    }

    // ===============================
    // EMAIL / PASSWORD REGISTER
    // ===============================
    public async Task<Response> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return Response.FailResponse(StaticDetails.MsgEmailAlreadyRegistered);

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return Response.FailResponse(StaticDetails.MsgRegistrationFailed);

        // Assign default role
        await _userManager.AddToRoleAsync(user, StaticDetails.RoleUser);

        // Send confirmation email
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = $"{_configuration["App:ClientUrl"]}/confirm-email?email={dto.Email}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendAsync(dto.Email, StaticDetails.EmailSubjectConfirm, string.Format(StaticDetails.EmailBodyConfirmSimpleTemplate, link));
        return Response.SuccessResponse(StaticDetails.MsgAccountCreated);
    }

    // ===============================
    // EMAIL / PASSWORD LOGIN
    // ===============================
    public async Task<AuthResponse> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException(StaticDetails.MsgInvalidCredentials);

        if (await _userManager.IsLockedOutAsync(user))
            throw new UnauthorizedAccessException("This account has been deleted.");
        
        if (!user.EmailConfirmed)
            throw new ApplicationException(StaticDetails.MsgEmailNotConfirmed);

        var roles = await _userManager.GetRolesAsync(user);
        return await _jwtService.GenerateTokens(user, roles);
    }

    // ===============================
    // GOOGLE / FACEBOOK LOGIN
    // ===============================
    public async Task<AuthResponse> SocialLoginAsync(string email, string fullName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Register new social user
            user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FullName = fullName,
                EmailConfirmed = true // already verified by Google/Facebook
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, StaticDetails.RoleUser);
        }

        var roles = await _userManager.GetRolesAsync(user);
        return await _jwtService.GenerateTokens(user, roles);
    }

    // ===============================
    // REFRESH TOKEN
    // ===============================
    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        => await _jwtService.RefreshAsync(refreshToken);

    // ===============================
    // CONFIRM EMAIL
    // ===============================
    public async Task ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new ApplicationException(StaticDetails.MsgInvalidEmail);

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) throw new ApplicationException(StaticDetails.MsgEmailConfirmationFailed);
    }

    // ===============================
    // Resend Email Confirmation
    // ===============================

    public async Task<Response> ResendEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        // Security: Do not reveal if user exists
        if (user == null)
            return Response.SuccessResponse(StaticDetails.MsgConfirmationEmailSentIfExists);

        if (user.EmailConfirmed)
            return Response.FailResponse(StaticDetails.MsgEmailAlreadyConfirmed);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink =
            $"{_configuration["App:ClientUrl"]}/confirm-email" +
            $"?email={Uri.EscapeDataString(user.Email!)}" +
            $"&token={Uri.EscapeDataString(token)}";

        await _emailService.SendAsync(
            user.Email!,
            StaticDetails.EmailSubjectConfirm,
            string.Format(StaticDetails.EmailBodyConfirmTemplate, user.FullName, confirmationLink)
        );

        return Response.SuccessResponse(StaticDetails.MsgConfirmationEmailResent);
    }


    // ===============================
    // FORGOT PASSWORD
    // ===============================
    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = $"{_configuration["App:ClientUrl"]}/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendAsync(email, StaticDetails.EmailSubjectReset, string.Format(StaticDetails.EmailBodyResetTemplate, link));
    }

    // ===============================
    // RESET PASSWORD
    // ===============================
    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) throw new ApplicationException(StaticDetails.MsgInvalidEmail);

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded)
            throw new ApplicationException(StaticDetails.MsgPasswordResetFailed);
    }

    // ===============================
    // Google Login
    // ===============================

    public async Task<AuthResponse> GoogleLoginAsync(GoogleLoginDto dto)
    {
        var oAuthService = new OAuthService(_configuration);
        var (email, fullName) = await oAuthService.VerifyGoogleTokenAsync(dto.IdToken);
        return await SocialLoginAsync(email, fullName);
    }

    // ===============================
    // Facebook Login
    // ===============================

    public async Task<AuthResponse> FacebookLoginAsync(FacebookLoginDto dto)
    {
        var oAuthService = new OAuthService(_configuration);
        var (email, fullName) = await oAuthService.VerifyFacebookTokenAsync(dto.AccessToken);
        return await SocialLoginAsync(email, fullName);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        if (result.Succeeded)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.UpdateSecurityStampAsync(user);
        }

        return result.Succeeded;
    }
}