using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;

namespace Phrase_App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverlaySettingController : ControllerBase
    {
        private readonly IOverlaySettingService _overlaySettingService;
        public OverlaySettingController(IOverlaySettingService overlaySettingService)
        {
            _overlaySettingService = overlaySettingService;
        }

        [Authorize]
        [HttpPost("save-overlay-settings")]
        public async Task<IActionResult> UpdateOverlaySettings([FromForm] OverlaySettingsDto dto)
        {
            var success = await _overlaySettingService.SaveOverlaySettingsAsync(User.GetUserId(), dto);

            if (success)
            {
                return Ok(new
                {
                    success = true,
                    message = "Overlay style applied successfully!"
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Failed to save overlay configuration."
            });
        }

        [Authorize]
        [HttpGet("overlay-settings")]
        public async Task<IActionResult> GetOverlaySettings()
        {
            var settings = await _overlaySettingService.GetOverlaySettingsAsync(User.GetUserId());
            return Ok(settings);
        }

    }
}
