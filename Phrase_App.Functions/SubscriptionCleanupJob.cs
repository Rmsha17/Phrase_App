using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Phrase_App.Functions
{

    public class SubscriptionCleanupJob
    {
        private readonly PhraseDbContext _context;
        private readonly ILogger _logger;

        public SubscriptionCleanupJob(PhraseDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<SubscriptionCleanupJob>();
        }

        // 🟢 Runs every day at 12:00 AM
        [Function("DowngradeExpiredSubscriptions")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Subscription cleanup started at: {DateTime.Now}");

            // 1. Find all users who are marked Premium but have an expired date
            var expiredUsers = await _context.Users
                .Where(u => u.IsPremium && u.PremiumExpiryDate < DateTime.UtcNow)
                .ToListAsync();

            foreach (var user in expiredUsers)
            {
                _logger.LogInformation($"Downgrading user: {user.Email}");
                await DowngradeProcessAsync(user.Id);
            }

            await _context.SaveChangesAsync();
        }

        private async Task DowngradeProcessAsync(string userId)
        {
            // 1. Update Premium Status
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return;

            var userIdGuid = Guid.Parse(user.Id);

            user.IsPremium = false;
            user.SubscriptionType = "Free";

            // 2. 🟢 Soft Delete Excess Quotes (Keep only 5)
            var excessQuotes = await _context.UserQuotes
                .Where(q => q.UserId == userIdGuid && q.IsActive)
                .OrderByDescending(q => q.CreatedAt)
                .Skip(5)
                .ToListAsync();

            foreach (var quote in excessQuotes)
            {
                quote.IsActive = false; // Just hide it
            }

            // 3. 🟢 Soft Delete Excess Custom Quotes (Keep only 1)
            var excessCustom = await _context.UserQuotes
                .Where(q => q.UserId == userIdGuid && q.QuoteId == null && q.IsActive)
                .OrderByDescending(q => q.CreatedAt)
                .Skip(1)
                .ToListAsync();

            foreach (var q in excessCustom)
            {
                q.IsActive = false;
            }

            // 4. 🟢 Soft Delete Excess Schedules (Keep only 2)
            var excessSchedules = await _context.QuoteSchedules
                                                .Where(s => s.UserId == userIdGuid && s.IsActive)
                                                .OrderByDescending(s => s.CreatedAt)
                                                .Skip(2)
                                                .ToListAsync();

            foreach (var s in excessSchedules)
            {
                s.IsActive = false;
            }

            // 5. Reset Overlay Style to Defaults
            var settings = await _context.OverlaySettings.FirstOrDefaultAsync(s => s.UserId == userIdGuid);
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
