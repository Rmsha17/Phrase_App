namespace Phrase_App.Core.DTOs.Request
{
    public class GooglePubSubNotification
    {
        public MessageData message { get; set; }
        public string subscription { get; set; }
    }

    public class MessageData
    {
        public string data { get; set; } // Base64 encoded string
        public string messageId { get; set; }
        public DateTime publishTime { get; set; }
    }

    // Once decoded, it looks like this:
    public class DeveloperNotification
    {
        public string version { get; set; }
        public string packageName { get; set; }
        public long eventTimeMillis { get; set; }
        public SubscriptionNotification subscriptionNotification { get; set; }
    }

    public class SubscriptionNotification
    {
        public string version { get; set; }
        public int notificationType { get; set; }
        // 2 = RENEWED, 3 = CANCELED, 13 = EXPIRED
        public string purchaseToken { get; set; }
        public string subscriptionId { get; set; }
    }
}
