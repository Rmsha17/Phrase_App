using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.Interfaces;

namespace Phrase_App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost("error")]
        public async Task<IActionResult> LogError([FromBody] ErrorLog log)
        {
            if (log == null) return BadRequest("Invalid log data");
            var isSaved = await _logService.LogErrorAsync(log);
            if (isSaved)
                return Ok(new { message = "Error logged successfully" });
            
            return StatusCode(500, "Failed to save error log to database");
        }
    }
}
