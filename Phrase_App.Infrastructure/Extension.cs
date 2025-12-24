using Microsoft.Extensions.DependencyInjection;

namespace Phrase_App.Infrastructure
{
    public static class Extension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
