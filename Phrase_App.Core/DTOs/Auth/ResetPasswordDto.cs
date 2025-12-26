namespace Phrase_App.Core.DTOs.Auth;

public record ResetPasswordDto(
    string Email,
    string Token,
    string NewPassword
);
