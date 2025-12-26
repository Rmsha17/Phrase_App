using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Phrase_App.Core.DTOs.Auth;
using Phrase_App.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PhraseDbContext _context;

    public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager, PhraseDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthResponse> GenerateTokens(ApplicationUser user, IList<string> roles)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryMinutes = int.Parse(jwtSettings["DurationInMinutes"]);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateSecureRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = token.ValidTo
        };
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken)
    {
        var existingToken = await _context.RefreshTokens
                                          .Include(x => x.User)
                                          .FirstOrDefaultAsync(x =>
                                            x.Token == refreshToken &&
                                            !x.IsRevoked);

        if (existingToken == null)
            throw new SecurityTokenException("Invalid refresh token");

        if (existingToken.ExpiryDate <= DateTime.UtcNow)
            throw new SecurityTokenException("Refresh token expired");

        existingToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(existingToken.User);
        return await GenerateTokens(existingToken.User, roles);
    }

    private static string GenerateSecureRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
