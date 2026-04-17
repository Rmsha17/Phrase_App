using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs;
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

    // PUT: api/scheduler/weekly/{id}
    [HttpPost("weekly/{id}")]
    public async Task<IActionResult> UpdateWeeklySchedule(Guid id, [FromBody] UpdateWeeklyScheduleDto dto)
    {
        var result = await _schedulerService.UpdateWeeklyAsync(id, dto, User.GetUserId());
        if (!result) return BadRequest(new { success = false, message = "Failed to update schedule" });
        return Ok(new { success = true, message = "Schedule updated successfully" });
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

    // POST: api/scheduler/select-active
    // Replaces all schedules with 24/7 all-days for the selected quotes.
    // User just picks quotes — scheduling is hidden.
    [HttpPost("select-active")]
    public async Task<IActionResult> SelectActiveQuotes([FromBody] SelectActiveQuotesDto dto)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _schedulerService.SelectActiveQuotesAsync(dto.UserQuoteIds, userId.Value);

        if (!result) return BadRequest(new { success = false, message = "Failed to update selection" });

        return Ok(new { success = true, message = "Selection saved" });
    }

    // GET: api/scheduler/quotes-with-selection
    // Returns all user quotes with IsSelected flag.
    [HttpGet("quotes-with-selection")]
    public async Task<IActionResult> GetQuotesWithSelection()
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _schedulerService.GetUserQuotesWithSelectionAsync(userId.Value);
        return Ok(result);
    }
}