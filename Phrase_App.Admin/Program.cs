using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Admin.Middleware;
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

            // Identity hardening
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password policy - tighten for production
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                // Lockout to mitigate brute force
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<PhraseDbContext>()
                .AddDefaultTokenProviders();
            // Secure cookie for Identity
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "PhraseAdminAuth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;

                // Optional: regenerate session on sign-in to prevent fixation
                options.Events = new CookieAuthenticationEvents
                {
                    OnSigningIn = context =>
                    {
                        // additional checks or logging
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            });

            // Antiforgery (AJAX use header X-CSRF-TOKEN)
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // CORS - restrict admin origins via config (set AdminClientUrl in production)
            var adminOrigin = builder.Configuration["AdminClientUrl"];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AdminCors", p =>
                {
                    if (!string.IsNullOrWhiteSpace(adminOrigin))
                        p.WithOrigins(adminOrigin).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    else
                        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // dev fallback; replace in prod
                });
            });

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AdminCors", p =>
            //    {
            //        p.AllowAnyOrigin()
            //         .AllowAnyHeader()
            //         .AllowAnyMethod();
            //    });
            //});

            builder.Services.AddMemoryCache();
            builder.Services.RegisterServices();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Seed default admin account (if not exists)
            SeedAdminAccount(app);
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            // Insert security headers middleware
            app.Use(async (context, next) =>
            {
                // Prevent MIME sniffing
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                // Prevent framing
                context.Response.Headers["X-Frame-Options"] = "DENY";
                // Referrer policy
                context.Response.Headers["Referrer-Policy"] = "no-referrer";
                // XSS protection (legacy)
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                // Content Security Policy - adjust to your allowed CDNs/assets
                // Find this line in your middleware section:
                context.Response.Headers["Content-Security-Policy"] =
                    "default-src 'self'; " +
                    "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " + // Added 'unsafe-inline'
                    "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
                    "img-src 'self' data:; " +
                    "font-src 'self' https://cdn.jsdelivr.net;" +
                    "connect-src 'self' ws://localhost:* http://localhost:*;"; ;
                await next();
            });

            app.UseCors("AdminCors");

            app.UseAuthentication();
            app.UseAuthorization();

            // Area route (must be registered before default routes)
            app.MapControllerRoute(
                name: "root",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Root -> Admin login (make login load by default)
            app.MapControllerRoute(
                name: "root",
                pattern: "",
                defaults: new { controller = "Account", action = "Login" });

            // Default MVC route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }

        private static void SeedAdminAccount(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var config = services.GetRequiredService<IConfiguration>();

                    var adminEmail = config["AdminUser:Email"] ?? "admin@believein.com";
                    var adminPassword = config["AdminUser:Password"] ?? "Admin28!!";
                    var adminRole = config["AdminUser:Role"] ?? "Admin";
                    var adminFullName = config["AdminUser:FullName"] ?? "Administrator";

                    // Ensure role exists
                    if (!roleManager.RoleExistsAsync(adminRole).GetAwaiter().GetResult())
                    {
                        var roleResult = roleManager.CreateAsync(new IdentityRole(adminRole)).GetAwaiter().GetResult();
                        if (!roleResult.Succeeded)
                            logger.LogWarning("Failed to create role {Role}: {Errors}", adminRole, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }

                    // Ensure admin user exists
                    var existing = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                    if (existing == null)
                    {
                        var adminUser = new ApplicationUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            EmailConfirmed = true,
                            FullName = adminFullName
                        };

                        var createResult = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
                        if (createResult.Succeeded)
                        {
                            var addRoleResult = userManager.AddToRoleAsync(adminUser, adminRole).GetAwaiter().GetResult();
                            if (!addRoleResult.Succeeded)
                                logger.LogWarning("Failed to add admin user to role: {Errors}", string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                            else
                                logger.LogInformation("Default admin user '{Email}' created and assigned to role '{Role}'", adminEmail, adminRole);
                        }
                        else
                        {
                            logger.LogWarning("Failed to create admin user {Email}: {Errors}", adminEmail, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        logger.LogInformation("Admin user {Email} already exists", adminEmail);
                        // ensure role membership
                        if (!userManager.IsInRoleAsync(existing, adminRole).GetAwaiter().GetResult())
                        {
                            var addRoleResult = userManager.AddToRoleAsync(existing, adminRole).GetAwaiter().GetResult();
                            if (!addRoleResult.Succeeded)
                                logger.LogWarning("Failed to add existing user to role {Role}: {Errors}", adminRole, string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger?.LogError(ex, "Error during admin account seeding");
                }
            }
        }
    }
}