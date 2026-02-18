using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;
using System.Security.Claims;
using System.Text;

namespace Phrase_App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config; 

        public PaymentController(IPaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
            _config = config; 
        }

        [HttpPost("verify-purchase")]
        public async Task<IActionResult> VerifyPurchase([FromBody] VerifyPurchaseRequest request)
        {
            var result = await _paymentService.VerifyAndUnlockPremiumAsync(User.GetUserId(), request);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("add-schedule")]
        public async Task<IActionResult> CanAddSchedule()
        {
            var result = await _paymentService.CanAddSchedule(User.GetUserId());
            if (!result) return BadRequest(new { success = false, message = "Failed" });

            return Ok(new { success = true, message = "Successfull" });
        }

        [HttpPost("custom-quote")]
        public async Task<IActionResult> CanAddCustomQuote()
        {
            var result = await _paymentService.CanAddCustomQuote(User.GetUserId());
            if (!result) return BadRequest(new { success = false, message = "Failed" });

            return Ok(new { success = true, message = "Successfull" });
        }

        /// Google Pub/Sub pushes subscription events here in real-time.
        /// Must return 200 ALWAYS — otherwise Pub/Sub retries endlessly.
        [HttpPost("google-notification")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleGoogleNotification(
            [FromBody] GooglePubSubMessage message)
        {
            try
            {
                var result = await _paymentService.HandleGooglePlayNotificationAsync(message);
                return Ok(new { result.Success, result.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RTDN Controller] Error: {ex.Message}");
                return Ok(new { Success = false, Message = "Error logged" });
            }
        }

        /// Daily cron safety net — protected with shared secret.
        [HttpPost("verify-all-subscriptions")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAllSubscriptions([FromHeader(Name = "X-Cron-Secret")] string cronSecret)
        {
            var expectedSecret = _config["CronJob:Secret"];
            if (string.IsNullOrEmpty(expectedSecret) || cronSecret != expectedSecret)
            {
                return Unauthorized("Invalid cron secret");
            }

            try
            {
                await _paymentService.VerifyAllActiveSubscriptionsAsync();
                return Ok(new { Success = true, Message = "Verification complete" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cron Controller] Error: {ex.Message}");
                return Ok(new { Success = false, Message = ex.Message });
            }
        }
    }
}