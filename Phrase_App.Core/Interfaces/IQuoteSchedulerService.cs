using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface IQuoteSchedulerService
{
    Task<bool> ScheduleWeeklyAsync(WeeklyScheduleRequestDto dto);
    Task<UserQuoteResponseDto?> GetActiveScheduledQuoteAsync(Guid userId);
    Task<IEnumerable<ScheduleResponseDto>> GetUserSchedulesAsync(Guid userId);
    Task<bool> RemoveScheduleAsync(Guid scheduleId);
}