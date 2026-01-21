using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Request;
using System.Security.Claims;

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

    [Authorize]
    [HttpGet("userDetail")]
    public async Task<IActionResult> GetUserDetails()
    {
        var response = await _authService.GetUserDetails(User.GetUserId());
        return Ok(response);
    }

    [Authorize]
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDtoRequest dto)
    {
        // Extract User ID from JWT Token
        var success = await _authService.UpdateUserProfile(User.GetUserId(), dto);

        if (success)
            return Ok(new { message = "Profile updated successfully!" });

        return BadRequest(new { message = "Failed to update profile details." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);
            // Wrap it in a 'data' field so your Flutter BaseService can find it!
            return Ok(new { success = true, message = "Login successful", data = response });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(dto.RefreshToken))
            return BadRequest(new { success = false, message = "Refresh token is required" });

        var result = await _authService.RevokeRefreshToken(userId, dto.RefreshToken);

        return Ok(new
        {
            success = true,
            message = "Logged out successfully from this device.",
            data = (object)null
        });
    }

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
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        await _authService.SendOtpAsync(dto.Email);
        return Ok(new { success = true, message = "OTP sent to your email" });
    }

    [HttpPost("verify-otp")]
    public IActionResult VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var isValid = _authService.VerifyOtp(dto.Email, dto.Otp);
        if (!isValid) return BadRequest(new { success = false, message = "Invalid or expired OTP" });

        return Ok(new { success = true, message = "OTP Verified" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        if (!result.Succeeded)
            return BadRequest(new { success = false, message = "Could not reset password." });

        return Ok(new { success = true, message = "Password updated successfully!" });
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

    [Authorize]
    [HttpDelete("delete-my-account")]
    public async Task<IActionResult> DeleteMyAccount()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = Guid.Parse(userIdClaim.Value);
        var success = await _authService.DeleteUserAsync(userId);

        if (success)
            return Ok(new { message = "Account has been deactivated and will be removed shortly." });

        return BadRequest("An error occurred while trying to delete the account.");
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _authService.UpdateProfileAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var result = await _authService.ChangePasswordAsync(User.GetUserId(), dto);
        if (result.Succeeded)
        {
            return Ok(new { success = true, message = "Password updated successfully!" });
        }

        return BadRequest(new
        {
            success = false,
            message = result.Errors.FirstOrDefault()?.Description ?? "Failed to update password."
        });
    }

    [Authorize]
    [HttpPost("request-email-change")]
    public async Task<IActionResult> RequestEmailChange(UpdateEmailDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _authService.RequestEmailChangeAsync(userId, dto.NewEmail);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        var result = await _authService.InitiateChangeEmailAsync(User.GetUserId(), dto);

        if (result.Succeeded)
        {
            return Ok(new
            {
                success = true,
                message = "Email Changed!"
            });
        }

        return BadRequest(new
        {
            success = false,
            message = result.Errors.FirstOrDefault()?.Description ?? "Failed to initiate email change."
        });
    }

    [Authorize]
    [HttpPost("toggle-darkmode")]
    public async Task<IActionResult> ToggleDarkMode([FromBody] ToggleDarkModeDto dto)
    {
        var success = await _authService.UpdateUserThemeAsync(User.GetUserId(), dto.IsDarkMode);

        if (success)
        {
            return Ok(new
            {
                success = true,
                message = "Theme preference updated successfully!"
            });
        }

        return BadRequest(new
        {
            success = false,
            message = "Failed to update theme preference."
        });
    }
}
