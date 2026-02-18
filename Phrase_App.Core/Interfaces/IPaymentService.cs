using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;

namespace Phrase_App.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> CanAddCustomQuote(Guid? userId);
        Task<bool> CanAddSchedule(Guid? userId);

        Task<Response> VerifyAndUnlockPremiumAsync(Guid? userId, VerifyPurchaseRequest request);

        /// Handles incoming Google Play RTDN Pub/Sub push notifications.
        Task<Response> HandleGooglePlayNotificationAsync(GooglePubSubMessage message);

        /// Called by RTDN webhook — handles all subscription state changes.
        //Task ProcessSubscriptionNotification(SubscriptionNotification notification);

        /// Daily safety-net cron job — re-verifies all active monthly subscriptions.
        Task VerifyAllActiveSubscriptionsAsync();

        /// Revokes premium and prunes resources back to free tier limits.
        Task DowngradeToFreeTierAsync(Guid? userId);
    }
}