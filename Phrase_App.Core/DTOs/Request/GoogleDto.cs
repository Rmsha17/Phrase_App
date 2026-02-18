using System.Text.Json.Serialization;

namespace Phrase_App.Core.DTOs.Request
{
    // ═══════════════════════════════════════════════════════════
    //  GOOGLE PUB/SUB WRAPPER
    //  This is what Google sends to your push endpoint
    // ═══════════════════════════════════════════════════════════

    public class GooglePubSubMessage
    {
        [JsonPropertyName("message")]
        public PubSubMessageBody Message { get; set; }

        [JsonPropertyName("subscription")]
        public string Subscription { get; set; }
    }

    public class PubSubMessageBody
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        [JsonPropertyName("publishTime")]
        public string PublishTime { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    //  DEVELOPER NOTIFICATION (decoded from Base64 Data)
    // ═══════════════════════════════════════════════════════════

    public class DeveloperNotifications
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("packageName")]
        public string PackageName { get; set; }

        [JsonPropertyName("eventTimeMillis")]
        public string EventTimeMillis { get; set; }

        [JsonPropertyName("subscriptionNotification")]
        public SubscriptionNotifications SubscriptionNotification { get; set; }

        [JsonPropertyName("oneTimeProductNotification")]
        public OneTimeProductNotification OneTimeProductNotification { get; set; }

        [JsonPropertyName("testNotification")]
        public TestNotification TestNotification { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    //  SUBSCRIPTION NOTIFICATION
    //
    //  NotificationType values:
    //   1  = RECOVERED           (payment fixed after hold)
    //   2  = RENEWED             (charged again)
    //   3  = CANCELED            (user turned off auto-renew)
    //   4  = PURCHASED           (new subscription)
    //   5  = ON_HOLD             (payment failed, grace ended)
    //   6  = IN_GRACE_PERIOD     (payment failed, retrying)
    //   7  = RESTARTED           (re-subscribed after cancel)
    //   8  = PRICE_CHANGE_CONFIRMED
    //   9  = DEFERRED
    //  10  = PAUSED
    //  11  = PAUSE_SCHEDULE_CHANGED
    //  12  = REVOKED             (refunded)
    //  13  = EXPIRED             (period ended, no renewal)
    //  20  = PENDING_PURCHASE_CANCELED
    // ═══════════════════════════════════════════════════════════

    public class SubscriptionNotifications
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("notificationType")]
        public int NotificationType { get; set; }

        [JsonPropertyName("purchaseToken")]
        public string PurchaseToken { get; set; }

        [JsonPropertyName("subscriptionId")]
        public string SubscriptionId { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    //  ONE-TIME PRODUCT NOTIFICATION
    // ═══════════════════════════════════════════════════════════

    public class OneTimeProductNotification
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("notificationType")]
        public int NotificationType { get; set; }

        [JsonPropertyName("purchaseToken")]
        public string PurchaseToken { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    //  TEST NOTIFICATION
    // ═══════════════════════════════════════════════════════════

    public class TestNotification
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}