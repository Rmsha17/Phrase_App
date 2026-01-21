using Microsoft.AspNetCore.Identity;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface IAuthService
{
    Task<Response> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
    Task<AuthResponse> SocialLoginAsync(string email, string fullName);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task ConfirmEmailAsync(string email, string token);
    Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto);
    Task<AuthResponse> GoogleLoginAsync(GoogleLoginDto dto);
    Task<AuthResponse> FacebookLoginAsync(FacebookLoginDto dto);
    Task<Response> ResendEmailConfirmationAsync(string email);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<Response> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task<Response> RequestEmailChangeAsync(Guid userId, string newEmail);
    Task<bool> RevokeRefreshToken(string userId, string refreshToken);
    bool VerifyOtp(string email, string otp);
    Task SendOtpAsync(string email);
    Task<UserDetailsDto> GetUserDetails(Guid? userId);
    Task<bool> UpdateUserProfile(Guid? userId, UpdateProfileDtoRequest dto);
    Task<IdentityResult> InitiateChangeEmailAsync(Guid? userId, ChangeEmailDto dto);
    Task<IdentityResult> ChangePasswordAsync(Guid? userId, ChangePasswordDto dto);
    Task<bool> UpdateUserThemeAsync(Guid? userId, bool isDarkMode);
}
