using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Models;
using System.Security.Claims;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly PhraseDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, IEmailService emailService, PhraseDbContext context, IConfiguration configuration, IMemoryCache cache)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _emailService = emailService;
        _context = context;
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<UserDetailsDto> GetUserDetails(Guid? userId)
    {
        var dateTime = DateTime.UtcNow;
        var idString = userId.Value.ToString();
        var user = await _userManager.Users
            .Where(u => u.Id == idString)
            .Select(u => new UserDetailsDto
            {
                UserName = u.FullName,
                Email = u.Email,
                ProfilePicUrl = u.ProfileImageUrl,
                DarkMode = u.DarkMode,
                IsPremium = u.IsPremium && u.PremiumExpiryDate > dateTime,
                PremiumExpiryDate = u.PremiumExpiryDate,
                SubscriptionType = u.SubscriptionType,
                CurrentPurchaseToken = u.CurrentPurchaseToken
            }).FirstOrDefaultAsync();

        return user;
    }

    public async Task<bool> UpdateUserProfile(Guid? userId, UpdateProfileDtoRequest dto)
    {
        var idString = userId.ToString();
        var user = await _userManager.FindByIdAsync(idString);

        if (user == null) return false;

        // Update the entity properties
        user.FullName = dto.FullName;
        user.Bio = dto.Bio; // Ensure your IdentityUser has a 'Bio' property
        user.ProfileImageUrl = dto.ProfilePicUrl;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
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
            throw new UnauthorizedAccessException("This account has been deactivated.");

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

        if (await _userManager.IsLockedOutAsync(user))
            throw new UnauthorizedAccessException("This account has been deactivated.");

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
    public async Task SendOtpAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return; // Security: don't reveal user doesn't exist

        var otp = new Random().Next(100000, 999999).ToString();

        var cacheKey = $"OTP_{email}";
        _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(10));

        await _emailService.SendAsync(email, "Reset Code", $"Your code is {otp}");
    }

    public bool VerifyOtp(string email, string otp)
    {
        var cacheKey = $"OTP_{email}";
        if (_cache.TryGetValue(cacheKey, out string storedOtp))
        {
            return storedOtp == otp;
        }

        return false;
    }

    // ===============================
    // RESET PASSWORD
    // ===============================

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return IdentityResult.Failed();

        // Remove the password and set a new one
        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, dto.NewPassword);

        // Clean up the cache after successful reset
        _cache.Remove($"OTP_{dto.Email}");

        return result;
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

    // ===============================
    // UPDATE PROFILE (Name & Image)
    // ===============================
    public async Task<Response> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return Response.FailResponse("User not found.");

        user.FullName = dto.FullName;
        if (!string.IsNullOrEmpty(dto.ProfileImageUrl))
        {
            user.ProfileImageUrl = dto.ProfileImageUrl;
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded
            ? Response.SuccessResponse("Profile updated successfully.")
            : Response.FailResponse("Failed to update profile.");
    }

    // ===============================
    // CHANGE PASSWORD
    // ===============================
    public async Task<IdentityResult> ChangePasswordAsync(Guid? userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        return result;
    }

    // ===============================
    // UPDATE EMAIL (Request Change)
    // ===============================
    public async Task<Response> RequestEmailChangeAsync(Guid userId, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return Response.FailResponse("User not found.");

        var existingUser = await _userManager.FindByEmailAsync(newEmail);
        if (existingUser != null) return Response.FailResponse("Email is already taken.");

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

        // Note: The link points to your Flutter App or a Web Redirector
        var link = $"{_configuration["App:ClientUrl"]}/confirm-email-change" +
                   $"?userId={userId}&newEmail={newEmail}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendAsync(newEmail, "Confirm Your New Email",
            $"Click here to confirm your email change: {link}");

        return Response.SuccessResponse("Confirmation link sent to your new email.");
    }

    public async Task<IdentityResult> InitiateChangeEmailAsync(Guid? userId, ChangeEmailDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Incorrect password. Verification failed." });
        }

        await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
        return IdentityResult.Success;
    }

    public async Task<bool> RevokeRefreshToken(string userId, string refreshToken)
    {
        // Find the specific token for this user in your database
        var tokenRecord = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == userId.ToString() && x.Token == refreshToken);

        if (tokenRecord == null) return false;
        _context.RefreshTokens.Remove(tokenRecord);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserThemeAsync(Guid? userId, bool isDarkMode)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return false;

        // Assuming your IdentityUser entity has a 'DarkMode' property
        user.DarkMode = isDarkMode;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<IdentityResult> DeactivateUserAsync(Guid? userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Incorrect password. Confirmation failed." });
        }

        await _userManager.SetLockoutEnabledAsync(user, true);
        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

        return result;
    }
}