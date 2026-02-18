using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;
using Phrase_App.Core.Models;
using System.Text;
using System.Text.Json;

namespace Phrase_App.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PhraseDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _jsonKeyPath;
        private readonly string _packageName;
        private readonly IConfiguration _config;

        public PaymentService(PhraseDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;

            var relativePath = _config["GooglePlay:ServiceAccountKeyPath"];
            _jsonKeyPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            _packageName = _config["GooglePlay:PackageName"];
        }

        // ═══════════════════════════════════════════════════════════
        //  FREE TIER LIMITS
        // ═══════════════════════════════════════════════════════════

        public async Task<bool> CanAddSchedule(Guid? userId)
        {
            var date = DateTime.UtcNow;
            var user = await GetUser(userId);
            if (user.IsPremium) return true;

            var scheduleCount = await _context.QuoteSchedules.CountAsync(s => s.Days.Any(d => d.DayOfWeek == (int)date.DayOfWeek) && s.UserId == userId);
            return scheduleCount < 2;
        }

        public async Task<bool> CanAddCustomQuote(Guid? userId)
        {
            var date = DateTime.UtcNow;

            var user = await GetUser(userId);
            if (user.IsPremium) return true;

            var customQuoteCount = await _context.UserQuotes.CountAsync(q => q.UserId == userId && q.QuoteId == null);
            return customQuoteCount < 1;
        }

        // ═══════════════════════════════════════════════════════════
        //  PURCHASE VERIFICATION (from Flutter app)
        // ═══════════════════════════════════════════════════════════

        public async Task<Response> VerifyAndUnlockPremiumAsync(Guid? userId, VerifyPurchaseRequest request)
        {
            try
            {
                bool isVerified = false;

                if (request.IsTest)
                {
                    if (request.PurchaseToken == "TEST_TOKEN_12345")
                    {
                        isVerified = true;
                    }
                }
                else
                {
                    isVerified = await VerifyWithGooglePlayAsync(request.ProductId, request.PurchaseToken);
                }

                if (isVerified)
                {
                    var user = await GetUser(userId);
                    if (user == null) return new Response { Success = false, Message = "User not found" };

                    user.CurrentPurchaseToken = request.PurchaseToken;

                    user.IsPremium = true;
                    user.SubscriptionType = request.ProductId.Contains("yearly") ? "Yearly" : "Monthly";
                    user.PremiumExpiryDate = request.ProductId.Contains("yearly")
                        ? DateTime.UtcNow.AddYears(1)
                        : DateTime.UtcNow.AddMonths(1);

                    ReactivateSoftDeletedData(userId);
                    await _context.SaveChangesAsync();

                    return new Response
                    {
                        Success = true,
                        Message = "Premium features unlocked successfully!"
                    };
                }

                return new Response { Success = false, Message = "Invalid purchase token" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Server Error: {ex.Message}" };
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  GOOGLE PLAY RTDN WEBHOOK (from Pub/Sub push)
        // ═══════════════════════════════════════════════════════════

        public async Task<Response> HandleGooglePlayNotificationAsync(GooglePubSubMessage message)
        {
            try
            {
                if (message?.Message?.Data == null)
                {
                    return new Response { Success = false, Message = "Empty notification" };
                }

                var decodedBytes = Convert.FromBase64String(message.Message.Data);
                var json = Encoding.UTF8.GetString(decodedBytes);

                Console.WriteLine($"[RTDN] Received: {json}");

                var notification = JsonSerializer.Deserialize<DeveloperNotifications>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (notification == null)
                {
                    return new Response { Success = false, Message = "Could not parse notification" };
                }

                // Handle subscription notifications
                if (notification.SubscriptionNotification != null)
                {
                    var subNotification = notification.SubscriptionNotification;

                    Console.WriteLine(
                        $"[RTDN] Type: {subNotification.NotificationType}, " +
                        $"SubId: {subNotification.SubscriptionId}, " +
                        $"Token: {subNotification.PurchaseToken?[..Math.Min(20, subNotification.PurchaseToken.Length)]}...");

                    await ProcessSubscriptionNotification(subNotification);

                    return new Response { Success = true, Message = "Notification processed" };
                }

                // Handle one-time purchase notifications
                if (notification.OneTimeProductNotification != null)
                {
                    Console.WriteLine(
                        $"[RTDN] One-time purchase notification: " +
                        $"Type: {notification.OneTimeProductNotification.NotificationType}");

                    return new Response { Success = true, Message = "One-time notification acknowledged" };
                }

                // Test notification from Play Console
                if (notification.TestNotification != null)
                {
                    Console.WriteLine($"[RTDN] Test notification received: {notification.TestNotification.Version}");
                    return new Response { Success = true, Message = "Test notification received" };
                }

                return new Response { Success = true, Message = "Notification type not handled" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RTDN] Error: {ex.Message}");
                return new Response { Success = false, Message = ex.Message };
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  SUBSCRIPTION STATE CHANGES (called by RTDN webhook)
        //  FIX: Parameter type is SubscriptionNotification (matches interface)
        // ═══════════════════════════════════════════════════════════

        public async Task ProcessSubscriptionNotification(SubscriptionNotifications notification)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.CurrentPurchaseToken == notification.PurchaseToken);

            if (user == null)
            {
                Console.WriteLine($"[RTDN] No user found for token: {notification.PurchaseToken?[..Math.Min(20, notification.PurchaseToken?.Length ?? 0)]}...");
                return;
            }

            Console.WriteLine($"[RTDN] Processing type {notification.NotificationType} for user {user.Id}");

            switch (notification.NotificationType)
            {
                case 1: // SUBSCRIPTION_RECOVERED
                    user.IsPremium = true;
                    ReactivateSoftDeletedData(Guid.Parse(user.Id));
                    await UpdateExpiryFromGoogleAsync(user, notification.SubscriptionId, notification.PurchaseToken);
                    break;

                case 2: // SUBSCRIPTION_RENEWED
                    var baseDate = user.PremiumExpiryDate > DateTime.UtcNow
                        ? user.PremiumExpiryDate.Value
                        : DateTime.UtcNow;

                    user.IsPremium = true;
                    user.PremiumExpiryDate = user.SubscriptionType == "Yearly"
                        ? baseDate.AddYears(1)
                        : baseDate.AddMonths(1);
                    break;

                case 3: // SUBSCRIPTION_CANCELED — don't revoke, paid until expiry
                    Console.WriteLine(
                        $"[RTDN] User {user.Id} canceled subscription. " +
                        $"Premium until: {user.PremiumExpiryDate}");
                    break;

                case 4: // SUBSCRIPTION_PURCHASED
                    user.IsPremium = true;
                    user.CurrentPurchaseToken = notification.PurchaseToken;
                    await UpdateExpiryFromGoogleAsync(user, notification.SubscriptionId, notification.PurchaseToken);
                    ReactivateSoftDeletedData(Guid.Parse(user.Id));
                    break;

                case 5: // SUBSCRIPTION_ON_HOLD
                    Console.WriteLine($"[RTDN] User {user.Id} subscription on hold");
                    break;

                case 6: // SUBSCRIPTION_IN_GRACE_PERIOD
                    Console.WriteLine($"[RTDN] User {user.Id} in grace period");
                    break;

                case 7: // SUBSCRIPTION_RESTARTED
                    user.IsPremium = true;
                    user.CurrentPurchaseToken = notification.PurchaseToken;
                    await UpdateExpiryFromGoogleAsync(user, notification.SubscriptionId, notification.PurchaseToken);
                    ReactivateSoftDeletedData(Guid.Parse(user.Id));
                    break;

                case 12: // SUBSCRIPTION_REVOKED
                    await DowngradeToFreeTierAsync(Guid.Parse(user.Id));
                    break;

                case 13: // SUBSCRIPTION_EXPIRED
                    await DowngradeToFreeTierAsync(Guid.Parse(user.Id));
                    break;

                default:
                    Console.WriteLine($"[RTDN] Unhandled notification type: {notification.NotificationType}");
                    break;
            }

            await _context.SaveChangesAsync();
        }

        // ═══════════════════════════════════════════════════════════
        //  CRON JOB: Daily safety net verification
        // ═══════════════════════════════════════════════════════════

        public async Task VerifyAllActiveSubscriptionsAsync()
        {
            Console.WriteLine("[CRON] Starting subscription verification...");

            var premiumUsers = await _userManager.Users
                .Where(u => u.IsPremium
                         && u.CurrentPurchaseToken != null
                         && u.SubscriptionType == "Monthly")
                .ToListAsync();

            Console.WriteLine($"[CRON] Checking {premiumUsers.Count} active monthly subscriptions");

            int revokedCount = 0;

            foreach (var user in premiumUsers)
            {
                try
                {
                    bool stillValid = await VerifyWithGooglePlayAsync(
                        "believein_monthly_sub",
                        user.CurrentPurchaseToken);

                    if (!stillValid)
                    {
                        Console.WriteLine($"[CRON] Revoking premium for user {user.Id}");
                        await DowngradeToFreeTierAsync(Guid.Parse(user.Id));
                        revokedCount++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CRON] Error checking user {user.Id}: {ex.Message}");
                }
            }

            var expiredUsers = await _userManager.Users
                .Where(u => u.IsPremium
                         && u.PremiumExpiryDate != null
                         && u.PremiumExpiryDate < DateTime.UtcNow
                         && u.SubscriptionType == "Monthly")
                .ToListAsync();

            foreach (var user in expiredUsers)
            {
                Console.WriteLine($"[CRON] Expiry-based revoke for user {user.Id} (expired: {user.PremiumExpiryDate})");
                await DowngradeToFreeTierAsync(Guid.Parse(user.Id));
                revokedCount++;
            }

            Console.WriteLine($"[CRON] Done. Revoked {revokedCount} subscriptions.");
        }

        // ═══════════════════════════════════════════════════════════
        //  DOWNGRADE
        // ═══════════════════════════════════════════════════════════

        public async Task DowngradeToFreeTierAsync(Guid? userId)
        {
            var user = await GetUser(userId);
            if (user == null) return;

            user.IsPremium = false;
            user.SubscriptionType = "Free";
            user.CurrentPurchaseToken = null;
            user.PremiumExpiryDate = null;

            await ToggleExcessiveResources(userId, false);

            await _context.SaveChangesAsync();
        }

        // ═══════════════════════════════════════════════════════════
        //  PRIVATE HELPERS
        // ═══════════════════════════════════════════════════════════

        private async Task<ApplicationUser?> GetUser(Guid? userId)
        {
            var idString = userId.ToString();
            var user = await _userManager.FindByIdAsync(idString);
            return user;
        }

        private async Task<bool> VerifyWithGooglePlayAsync(string productId, string token)
        {
            if (!File.Exists(_jsonKeyPath))
                throw new FileNotFoundException($"Google Service Account key not found at: {_jsonKeyPath}");

            GoogleCredential credential;
            using (var stream = new FileStream(_jsonKeyPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);
            }

            var service = new AndroidPublisherService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "BelieveIn App"
            });

            try
            {
                var subResult = await service.Purchases.Subscriptions
                    .Get(_packageName, productId, token).ExecuteAsync();

                return subResult.PaymentState == 1 || subResult.PaymentState == 2;
            }
            catch
            {
                try
                {
                    var productResult = await service.Purchases.Products
                        .Get(_packageName, productId, token).ExecuteAsync();

                    return productResult.PurchaseState == 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        private async Task UpdateExpiryFromGoogleAsync(
            ApplicationUser user, string subscriptionId, string purchaseToken)
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(_jsonKeyPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);
                }

                var service = new AndroidPublisherService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "BelieveIn App"
                });

                var subResult = await service.Purchases.Subscriptions
                    .Get(_packageName, subscriptionId, purchaseToken)
                    .ExecuteAsync();

                if (subResult.ExpiryTimeMillis.HasValue)
                {
                    user.PremiumExpiryDate = DateTimeOffset
                        .FromUnixTimeMilliseconds(subResult.ExpiryTimeMillis.Value)
                        .UtcDateTime;

                    Console.WriteLine($"[RTDN] Updated expiry for user {user.Id}: {user.PremiumExpiryDate}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RTDN] UpdateExpiry error for user {user.Id}: {ex.Message}");
            }
        }

        private void ReactivateSoftDeletedData(Guid? userId)
        {
            var deactivatedQuotes = _context.UserQuotes.Where(q => q.UserId == userId && !q.IsActive);
            foreach (var quote in deactivatedQuotes)
                quote.IsActive = true;

            var deactivatedSchedules = _context.QuoteSchedules.Where(s => s.UserId == userId && !s.IsActive);
            foreach (var schedule in deactivatedSchedules)
                schedule.IsActive = true;
        }

        private async Task ToggleExcessiveResources(Guid? userId, bool check)
        {
            var excessQuotes = _context.UserQuotes
                .Where(q => q.UserId == userId && q.QuoteId != null)
                .OrderByDescending(q => q.CreatedAt)
                .Skip(5);
            await excessQuotes.ForEachAsync(q => q.IsActive = false);

            var excessCustomQuotes = _context.UserQuotes
                .Where(q => q.UserId == userId && q.QuoteId == null)
                .OrderByDescending(q => q.CreatedAt)
                .Skip(1);
            await excessCustomQuotes.ForEachAsync(q => q.IsActive = check);

            var excessSchedules = _context.QuoteSchedules
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Skip(2);
            await excessSchedules.ForEachAsync(s => s.IsActive = check);

            var settings = await _context.OverlaySettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (settings != null)
            {
                settings.FontSize = 14.0;
                settings.FontColor = "FFFFFFFF";
                settings.FontFamily = "Default";
                settings.Opacity = 1.0;
                settings.BackgroundType = "Glass";
                settings.BackgroundValue = null;
                settings.AnimationType = "Fade";
                settings.Position = "Center";
                settings.DisplayMode = "Compact Box";
                settings.VibrationEnabled = true;
                settings.SoundEffect = "Default";
                settings.IntervalMinutes = 1;
            }
        }
    }
}