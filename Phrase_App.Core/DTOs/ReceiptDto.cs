namespace Phrase_App.Core.DTOs
{
    public class VerifyPurchaseRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public string PurchaseToken { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public bool IsTest { get; set; } = false; // 🟢 To handle mock testing
    }
}
