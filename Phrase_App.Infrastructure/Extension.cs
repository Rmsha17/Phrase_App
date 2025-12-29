using Microsoft.Extensions.DependencyInjection;
using Phrase_App.Core.Interfaces;
using Phrase_App.Infrastructure.Services;

namespace Phrase_App.Infrastructure
{
    public static class Extension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserQuoteService, UserQuoteService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
