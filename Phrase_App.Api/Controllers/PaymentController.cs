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

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
    }
}
