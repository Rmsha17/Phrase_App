using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface IQuoteSchedulerService
{
    Task<bool> ScheduleWeeklyAsync(WeeklyScheduleRequestDto dto, Guid? userId);
    Task<List<UserQuoteResponseDto>> GetActiveScheduledQuoteAsync(Guid? userId);
    Task<IEnumerable<ScheduleResponseDto>> GetUserSchedulesAsync(Guid? userId);
    Task<bool> RemoveScheduleAsync(Guid scheduleId);
    Task<List<UserQuoteResponseDto>> GetCurrentScheduledQuoteAsync(Guid? userId);
    Task<bool> UpdateWeeklyAsync(Guid scheduleId, UpdateWeeklyScheduleDto dto, Guid? userId);

    /// <summary>
    /// Replaces all existing schedules for the user with 24/7 all-days schedules
    /// for each selected quote. User never sees the schedule — it's automatic.
    /// </summary>
    Task<bool> SelectActiveQuotesAsync(List<Guid> userQuoteIds, Guid userId);

    /// <summary>
    /// Returns all user quotes with IsSelected=true if they have an active schedule.
    /// </summary>
    Task<List<UserQuoteWithSelectionDto>> GetUserQuotesWithSelectionAsync(Guid userId);
}