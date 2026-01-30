using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;
using Phrase_App.Core.Models;

namespace Phrase_App.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PhraseDbContext _context;
        private readonly string _packageName = "com.example.phrase_app"; // 🟢 Your Android Package Name
        private readonly string _jsonKeyPath = "path-to-your-google-service-account.json"; // 🟢 Path to your JSON key
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentService(PhraseDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> CanAddSchedule(Guid? userId)
        {
            var date = DateTime.UtcNow;
            var user = await GetUser(userId);
            if (user.IsPremium) return true;

            var scheduleCount = await _context.QuoteSchedules.CountAsync(s => s.Days.Any(d => d.DayOfWeek == (int)date.DayOfWeek) && s.UserId == userId);
            return scheduleCount < 2; // Limit to 2 for Free
        }

        public async Task<bool> CanAddCustomQuote(Guid? userId)
        {
            var date = DateTime.UtcNow;

            var user = await GetUser(userId);
            if (user.IsPremium) return true;

            var customQuoteCount = await _context.UserQuotes.CountAsync(q => q.UserId == userId && q.QuoteId == null);
            return customQuoteCount < 1; // Limit to 1 for Free
        }

        public async Task<Response> VerifyAndUnlockPremiumAsync(Guid? userId, VerifyPurchaseRequest request)
        {
            try
            {
                bool isVerified = false;

                if (request.IsTest)
                {
                    // 🟢 Handle Mock Testing from Flutter
                    if (request.PurchaseToken == "TEST_TOKEN_12345")
                    {
                        isVerified = true;
                    }
                }
                else
                {
                    // 🔴 REAL GOOGLE PLAY VERIFICATION LOGIC
                    // Here you would use Google.Apis.AndroidPublisher.v3 library
                    // to verify the token with Google Servers.
                    // For now, we assume it's true if not in test mode
                    isVerified = await VerifyWithGooglePlayAsync(request.ProductId, request.PurchaseToken);
                }

                if (isVerified)
                {
                    var user = await GetUser(userId);
                    if (user == null) return new Response { Success = false, Message = "User not found" };

                    // 🟢 Save the token so Webhooks can find this user later
                    user.CurrentPurchaseToken = request.PurchaseToken;

                    // 🟢 Update User Status
                    user.IsPremium = true;
                    user.SubscriptionType = request.ProductId.Contains("yearly") ? "Yearly" : "Monthly";
                    user.PremiumExpiryDate = request.ProductId.Contains("yearly")
                        ? DateTime.UtcNow.AddYears(1)
                        : DateTime.UtcNow.AddMonths(1);

                    // 2. 🟢 REACTIVATE ALL SOFT-DELETED RECORDS
                    // Find all quotes and schedules that were deactivated during downgrade
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

        private void ReactivateSoftDeletedData(Guid? userId)
        {
            var deactivatedQuotes = _context.UserQuotes.Where(q => q.UserId == userId && !q.IsActive);
            foreach (var quote in deactivatedQuotes)
                quote.IsActive = true;

            var deactivatedSchedules = _context.QuoteSchedules.Where(s => s.UserId == userId && !s.IsActive);
            foreach (var schedule in deactivatedSchedules)
                schedule.IsActive = true;
        }

        private async Task<ApplicationUser?> GetUser(Guid? userId)
        {
            var idString = userId.ToString();
            var user = await _userManager.FindByIdAsync(idString);
            return user;
        }

        private async Task<bool> VerifyWithGooglePlayAsync(string productId, string token)
        {
            // 1. Authenticate with Google
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
                // 2. Check if it's a Subscription (Monthly/Yearly Sub)
                // Use Subscriptions.Get for recurring billing
                var subResult = await service.Purchases.Subscriptions.Get(_packageName, productId, token).ExecuteAsync();

                // PaymentState: 1 = Purchased, 0 = Pending, 2 = Free Trial
                return subResult.PaymentState == 1 || subResult.PaymentState == 2;
            }
            catch
            {
                try
                {
                    // 3. Check if it's a Product (One-time purchase / Non-consumable)
                    var productResult = await service.Purchases.Products.Get(_packageName, productId, token).ExecuteAsync();

                    // PurchaseState: 0 = Purchased, 1 = Canceled, 2 = Pending
                    return productResult.PurchaseState == 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task ProcessSubscriptionNotification(SubscriptionNotification notification)
        {
            // 1. Find the user associated with this purchase token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.CurrentPurchaseToken == notification.purchaseToken);
            if (user == null) return;

            switch (notification.notificationType)
            {
                case 2: // 🟢 SUBSCRIPTION_RENEWED
                        // Google successfully charged the user again.
                        // Extend access from their CURRENT expiry date, not from Today.
                    var baseDate = user.PremiumExpiryDate > DateTime.UtcNow
                        ? user.PremiumExpiryDate.Value
                        : DateTime.UtcNow;

                    user.IsPremium = true;
                    user.PremiumExpiryDate = user.SubscriptionType == "Yearly"
                        ? baseDate.AddYears(1)
                        : baseDate.AddMonths(1);
                    break;

                case 3: // 🟡 SUBSCRIPTION_CANCELED
                        // User turned off auto-renew.
                        // 🟢 IMPORTANT: We do NOT set IsPremium = false here.
                        // They paid for time, so we let them use it until the ExpiryDate is reached.
                    break;

                case 13: // 🔴 SUBSCRIPTION_EXPIRED
                         // The grace period is over and the user didn't pay.
                         // Now we lock the features.
                    await DowngradeToFreeTierAsync(Guid.Parse(user.Id));
                    user.IsPremium = false;
                    break;

                case 1: // 🔵 SUBSCRIPTION_RECOVERED
                        // User was on hold (payment fail) but fixed their card.
                    user.IsPremium = true;
                    break;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DowngradeToFreeTierAsync(Guid? userId)
        {
            var user = await GetUser(userId);
            if (user == null) return;

            // 1. Update User Status
            user.IsPremium = false;
            user.SubscriptionType = "Free";
            user.CurrentPurchaseToken = null; // Clear token on expiry


            await ToggleExcessiveResources(userId, false);

            await _context.SaveChangesAsync();
        }

        private async Task ToggleExcessiveResources(Guid? userId, bool check)
        {
            // 2. Prune UserQuotes (Keep only 5)
            // We keep the 5 most recently added quotes
            var excessQuotes = _context.UserQuotes
                                       .Where(q => q.UserId == userId && q.QuoteId != null)
                                       .OrderByDescending(q => q.CreatedAt)
                                       .Skip(5);

            await excessQuotes.ForEachAsync(q => q.IsActive = false);

            // 3. Prune Custom Quotes (Keep only 1)
            var excessCustomQuotes = _context.UserQuotes
                                            .Where(q => q.UserId == userId && q.QuoteId == null)
                                            .OrderByDescending(q => q.CreatedAt)
                                            .Skip(1);

            await excessCustomQuotes.ForEachAsync(q => q.IsActive = check);

            // 4. Prune Schedules (Keep only 2)
            var excessSchedules = _context.QuoteSchedules
                                          .Where(s => s.UserId == userId)
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(2);

            await excessSchedules.ForEachAsync(s => s.IsActive = check);

            // 2. Reset Overlay Style to Defaults
            var settings = await _context.OverlaySettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (settings != null)
            {
                settings.FontSize = 14.0;
                settings.FontColor = "FFFFFFFF"; // White
                settings.FontFamily = "Default";
                settings.Opacity = 1.0;
                settings.BackgroundType = "Glass"; // Force back to free Glass texture
                settings.BackgroundValue = null;
                settings.AnimationType = "Fade";
                settings.Position = "Center";
                settings.DisplayMode = "Compact Box"; // Force back to Compact Box
                settings.VibrationEnabled = true;
                settings.SoundEffect = "Default";
                settings.IntervalMinutes = 1;
            }

        }
    }
}
