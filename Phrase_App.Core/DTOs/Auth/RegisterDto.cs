namespace MyApp.Core.DTOs.Auth;

public record RegisterDto(
    string FullName,
    string Email,
    string Password
);
