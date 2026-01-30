using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs.Request;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly IQuoteSchedulerService _schedulerService;

    public SchedulerController(IQuoteSchedulerService schedulerService)
    {
        _schedulerService = schedulerService;
    }

    // POST: api/scheduler/weekly
    [HttpPost("weekly")]
    public async Task<IActionResult> CreateWeeklySchedule([FromBody] WeeklyScheduleRequestDto dto)
    {
        var result = await _schedulerService.ScheduleWeeklyAsync(dto, User.GetUserId());

        if (!result) return BadRequest(new { success = false, message = "Failed to create schedule" });

        return Ok(new { success = true, message = "Schedule created successfully" });
    }

    // GET: api/scheduler/active/{userId}
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentScheduledQuoteAsync()
    {
        var quotes = await _schedulerService.GetCurrentScheduledQuoteAsync(User.GetUserId());
        return Ok(quotes);
    }

    // GET: api/scheduler/active/{userId}
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveQuote()
    {
        var quotes = await _schedulerService.GetActiveScheduledQuoteAsync(User.GetUserId());
        return Ok(quotes);
    }

    // GET: api/scheduler/user/{userId}
    [HttpGet("user")]
    public async Task<IActionResult> GetUserSchedules()
    {
        var schedules = await _schedulerService.GetUserSchedulesAsync(User.GetUserId());
        return Ok(schedules);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var result = await _schedulerService.RemoveScheduleAsync(id);

        if (!result) return BadRequest(new { success = false, message = "Failed to delete schedule" });

        return Ok(new { success = true, message = "Deleted successfully" });
    }
}