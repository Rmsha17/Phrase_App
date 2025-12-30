using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Request;

public interface IAuthService
{
    Task<Response> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
    Task<AuthResponse> SocialLoginAsync(string email, string fullName);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task ConfirmEmailAsync(string email, string token);
    Task ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(ResetPasswordDto dto);
    Task<AuthResponse> GoogleLoginAsync(GoogleLoginDto dto);
    Task<AuthResponse> FacebookLoginAsync(FacebookLoginDto dto);
    Task<Response> ResendEmailConfirmationAsync(string email);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<Response> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
    Task<Response> ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
    Task<Response> RequestEmailChangeAsync(Guid userId, string newEmail);
    Task<Response> ConfirmEmailChangeAsync(Guid userId, string newEmail, string token);
}
