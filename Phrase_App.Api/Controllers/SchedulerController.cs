using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.DTOs.Request;

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
        var result = await _schedulerService.ScheduleWeeklyAsync(dto);
        return result ? Ok(new { Message = "Schedule created successfully" }) : BadRequest("Failed to create schedule");
    }

    // GET: api/scheduler/active/{userId}
    [HttpGet("active/{userId}")]
    public async Task<IActionResult> GetActiveQuote(Guid userId)
    {
        var quote = await _schedulerService.GetActiveScheduledQuoteAsync(userId);
        if (quote == null) return NotFound("No quote scheduled for this time.");
        return Ok(quote);
    }

    // GET: api/scheduler/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserSchedules(Guid userId)
    {
        var schedules = await _schedulerService.GetUserSchedulesAsync(userId);
        return Ok(schedules);
    }

    // DELETE: api/scheduler/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var result = await _schedulerService.RemoveScheduleAsync(id);
        return result ? NoContent() : NotFound();
    }
}