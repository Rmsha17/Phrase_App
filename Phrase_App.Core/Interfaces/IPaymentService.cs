
using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;

namespace Phrase_App.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> CanAddCustomQuote(Guid? userId);
        Task<bool> CanAddSchedule(Guid? userId);
        Task ProcessSubscriptionNotification(SubscriptionNotification notification);
        Task<Response> VerifyAndUnlockPremiumAsync(Guid? userId, VerifyPurchaseRequest request);
    }
}
