using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

public class OAuthService
{
    private readonly IConfiguration _configuration;

    public OAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Google ID Token verification
    public async Task<(string Email, string FullName)> VerifyGoogleTokenAsync(string idToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        return (payload.Email, payload.Name);
    }

    // Facebook Access Token verification
    public async Task<(string Email, string FullName)> VerifyFacebookTokenAsync(string accessToken)
    {
        var appId = _configuration["Authentication:Facebook:AppId"];
        var appSecret = _configuration["Authentication:Facebook:AppSecret"];
        var client = new HttpClient();

        // Validate token
        var debugResponse = await client.GetFromJsonAsync<JsonElement>(
            $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appId}|{appSecret}");

        var data = debugResponse.GetProperty("data");
        if (!data.GetProperty("is_valid").GetBoolean())
            throw new ApplicationException("Invalid Facebook token");

        // Get user info
        var userInfo = await client.GetFromJsonAsync<JsonElement>(
            $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");

        string email = userInfo.GetProperty("email").GetString();
        string name = userInfo.GetProperty("name").GetString();

        return (email, name);
    }
}
