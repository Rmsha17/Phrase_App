using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Phrase_App.Core.Interfaces;

namespace Phrase_App.Infrastructure.Services
{
    /// <summary>
    /// Timer-triggered background service that verifies all active subscriptions daily.
    /// Calls PaymentService.VerifyAllActiveSubscriptionsAsync() directly in-process.
    /// No HTTP call, no cron secret needed.
    /// 
    /// Register in Program.cs:
    ///   builder.Services.AddHostedService&lt;SubscriptionVerificationService&gt;();
    /// </summary>
    public class SubscriptionVerificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionVerificationService> _logger;

        // ─── Configuration ───
        private static readonly TimeSpan RunTime = TimeSpan.FromHours(3); // 3:00 AM UTC daily
        private static readonly TimeSpan RetryDelay = TimeSpan.FromMinutes(30);
        private const int MaxRetries = 3;

        public SubscriptionVerificationService(IServiceProvider serviceProvider,
                                               ILogger<SubscriptionVerificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[SubVerify] Service started. Runs daily at {Time} UTC", RunTime);

            while (!stoppingToken.IsCancellationRequested)
            {
                // 1. Calculate delay until next 3:00 AM UTC
                var delay = GetDelayUntilNextRun();
                _logger.LogInformation(
                    "[SubVerify] Next run in {Hours:F1} hours ({NextRun} UTC)",
                    delay.TotalHours,
                    DateTime.UtcNow.Add(delay).ToString("yyyy-MM-dd HH:mm"));

                // 2. Wait until run time
                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break; // App shutting down
                }

                // 3. Execute with retry logic
                await RunWithRetryAsync(stoppingToken);
            }

            _logger.LogInformation("[SubVerify] Service stopped.");
        }

        private async Task RunWithRetryAsync(CancellationToken stoppingToken)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    _logger.LogInformation(
                        "[SubVerify] Starting verification (attempt {Attempt}/{Max})",
                        attempt, MaxRetries);

                    // New scope required — DbContext & UserManager are scoped services
                    using var scope = _serviceProvider.CreateScope();
                    var paymentService = scope.ServiceProvider
                        .GetRequiredService<IPaymentService>();

                    await paymentService.VerifyAllActiveSubscriptionsAsync();

                    _logger.LogInformation("[SubVerify] Verification completed successfully.");
                    return; // Success — exit retry loop
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "[SubVerify] Attempt {Attempt}/{Max} failed: {Message}",
                        attempt, MaxRetries, ex.Message);

                    if (attempt < MaxRetries)
                    {
                        _logger.LogInformation(
                            "[SubVerify] Retrying in {Minutes} minutes...",
                            RetryDelay.TotalMinutes);
                        try
                        {
                            await Task.Delay(RetryDelay, stoppingToken);
                        }
                        catch (TaskCanceledException)
                        {
                            return; // App shutting down
                        }
                    }
                    else
                    {
                        _logger.LogError(
                            "[SubVerify] All {Max} attempts failed. Will retry tomorrow.",
                            MaxRetries);
                    }
                }
            }
        }

        private TimeSpan GetDelayUntilNextRun()
        {
            var now = DateTime.UtcNow;
            var todayRun = now.Date.Add(RunTime);

            // If today's 3:00 AM already passed, schedule for tomorrow
            if (now >= todayRun)
                todayRun = todayRun.AddDays(1);

            return todayRun - now;
        }
    }
}