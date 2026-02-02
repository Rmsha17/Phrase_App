using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Models;
using Phrase_App.Infrastructure;

namespace Phrase_App.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Configure DbContext (adjust connection string name if different)
            builder.Services.AddDbContext<PhraseDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity using existing ApplicationUser and PhraseDbContext
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password policy tuned for admin UI convenience; adjust as needed
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<PhraseDbContext>()
                .AddDefaultTokenProviders();

            // Cookie settings for admin interactive UI
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "PhraseAdminAuth";
            });

            // Authentication & Authorization
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            // CORS for admin frontend (adjust for production origin)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AdminCors", p =>
                {
                    p.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                });
            });
            builder.Services.AddMemoryCache();
            builder.Services.RegisterServices();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AdminCors");


            app.UseAuthentication();
            app.UseAuthorization();

            // Area routing for Admin
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Reports}/{action=Dashboard}/{id?}");

            // Default MVC route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Enable Razor Pages (if you add any)
            app.MapRazorPages();

            app.Run();
        }
    }
}
