using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.DTOs.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Phrase_App.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authApi;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthService authApi, ILogger<AccountController> logger)
        {
            _authApi = authApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> DataDeletionPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PrivacyPolicy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // If authenticated and in Admin role -> go to Home
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home");
                }

                // Authenticated but not admin — sign out to avoid redirect loop
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                Response.Cookies.Delete("admin_auth_token");
                Response.Cookies.Delete("admin_refresh_token");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var response = await _authApi.LoginAsync(model);

                if (response == null || string.IsNullOrEmpty(response.AccessToken))
                {
                    ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                    return View(model);
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(response.AccessToken);

                // 1. Extract Roles
                var roleClaims = jwtToken.Claims
                    .Where(c => string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(c.Type, "roles", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase))
                    .Select(c => c.Value)
                    .ToList();

                // 2. RESTRICTION: Check if "Admin" exists in the roles
                // Change "Admin" to whatever your exact role string is (e.g., "Administrator")
                bool isAdmin = roleClaims.Any(r => string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase));

                if (!isAdmin)
                {
                    _logger.LogWarning("Unauthorized login attempt by user {Email}", model.Email);
                    ModelState.AddModelError(string.Empty, "Access Denied: You do not have Administrative privileges.");
                    return View(model);
                }

                // 3. Proceed with Identity creation if Admin
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;
                var name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "name" || c.Type == "email")?.Value
                           ?? model.Email ?? "Admin";

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, name) };
                if (!string.IsNullOrEmpty(userId)) claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

                // Add roles to Identity
                foreach (var role in roleClaims) claims.Add(new Claim(ClaimTypes.Role, role));

                claims.Add(new Claim("jwt_token", response.AccessToken));

                var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal,
                    new AuthenticationProperties { IsPersistent = true });

                // Set Cookies
                var cookieOptions = new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict };
                Response.Cookies.Append("admin_auth_token", response.AccessToken, cookieOptions);

                if (!string.IsNullOrEmpty(response.RefreshToken))
                    Response.Cookies.Append("admin_refresh_token", response.RefreshToken, cookieOptions);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                ModelState.AddModelError(string.Empty, "System error: Unable to authenticate.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Cookies["admin_auth_token"];
                var refreshToken = Request.Cookies["admin_refresh_token"];

                if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        try
                        {
                            await _authApi.RevokeRefreshToken(userId, refreshToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogWarning(e, "API logout/revoke failed (continuing local sign-out).");
                        }
                    }
                }

                Response.Cookies.Delete("admin_auth_token");
                Response.Cookies.Delete("admin_refresh_token");
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed");
                Response.Cookies.Delete("admin_auth_token");
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("Login", "Account");
            }
        }
    }
}