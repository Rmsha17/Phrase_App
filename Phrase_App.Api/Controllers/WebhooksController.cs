using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;
using System.Text;

namespace Phrase_App.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/webhooks")]
    public class WebhooksController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public WebhooksController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("google-play")]
        public async Task<IActionResult> HandleGoogleNotification([FromBody] GooglePubSubNotification request)
        {
            // 1. Decode the Base64 data from Google
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(request.message.data));
            var notification = JsonConvert.DeserializeObject<DeveloperNotification>(json);

            if (notification.subscriptionNotification != null)
            {
                // 2. Process the renewal or expiry
                await _paymentService.ProcessSubscriptionNotification(notification.subscriptionNotification);
            }

            return Ok();
        }
    }
}
