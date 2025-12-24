using MyApp.Core.DTOs.Auth;
using Phrase_App.Core.Models;

public interface IJwtService
{
    Task<AuthResponse> GenerateTokens(ApplicationUser user, IList<string> roles);
    Task<AuthResponse> RefreshAsync(string refreshToken);
}
