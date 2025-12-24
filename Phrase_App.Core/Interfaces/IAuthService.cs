using MyApp.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Auth;

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

}
