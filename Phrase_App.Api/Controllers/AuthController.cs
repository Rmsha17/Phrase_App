using Microsoft.AspNetCore.Mvc;
using MyApp.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var response = await _authService.RegisterAsync(dto);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
        => Ok(await _authService.LoginAsync(dto));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        => Ok(await _authService.RefreshTokenAsync(dto.RefreshToken));

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        await _authService.ConfirmEmailAsync(email, token);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await _authService.ForgotPasswordAsync(dto.Email);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto);
        return Ok();
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginDto dto)
    {
        var authResponse = await _authService.GoogleLoginAsync(dto);
        return Ok(authResponse);
    }

    [HttpPost("facebook-login")]
    public async Task<IActionResult> FacebookLogin(FacebookLoginDto dto)
    {
        var authResponse = await _authService.FacebookLoginAsync(dto);
        return Ok(authResponse);
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendEmailConfirmationDto dto)
    {
        var response = await _authService.ResendEmailConfirmationAsync(dto.Email);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
}
