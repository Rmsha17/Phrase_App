using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Interfaces;
using Phrase_App.Core.Models;
using System;

namespace Phrase_App.Infrastructure.Services
{
    public class QuoteSchedulerService : IQuoteSchedulerService
    {
        private readonly PhraseDbContext _context;

        public QuoteSchedulerService(PhraseDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ScheduleWeeklyAsync(WeeklyScheduleRequestDto dto)
        {
            var schedule = new QuoteSchedule
            {
                UserId = dto.UserId,
                UserQuoteId = dto.UserQuoteId,
                DailyStartTime = TimeSpan.Parse(dto.StartTime), // Expects "HH:mm"
                DailyEndTime = TimeSpan.Parse(dto.EndTime),
                Days = dto.DaysOfWeek.Select(day => new ScheduledDay
                {
                    DayOfWeek = day
                }).ToList()
            };

            _context.QuoteSchedules.Add(schedule);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserQuoteResponseDto?> GetActiveScheduledQuoteAsync(Guid userId)
        {
            var now = DateTime.Now; // Use local time for user-specific scheduling
            int currentDay = (int)now.DayOfWeek;
            var currentTime = now.TimeOfDay;

            var activeSchedule = await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                    .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId &&
                            s.Days.Any(d => d.DayOfWeek == currentDay) &&
                            currentTime >= s.DailyStartTime &&
                            currentTime <= s.DailyEndTime)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            if (activeSchedule == null) return null;

            return new UserQuoteResponseDto
            {
                Id = activeSchedule.UserQuote.Id,
                Content = activeSchedule.UserQuote.QuoteId != null
                          ? activeSchedule.UserQuote.Quote.Content
                          : activeSchedule.UserQuote.CustomContent,
                Author = activeSchedule.UserQuote.QuoteId != null
                         ? activeSchedule.UserQuote.Quote.Author
                         : activeSchedule.UserQuote.CustomAuthor
            };
        }

        public async Task<IEnumerable<ScheduleResponseDto>> GetUserSchedulesAsync(Guid userId)
        {
            return await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                    .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId)
                .Select(s => new ScheduleResponseDto
                {
                    Id = s.Id,
                    UserQuoteId = s.UserQuoteId,
                    // Fetch content from system or custom source
                    QuoteContent = s.UserQuote.QuoteId != null
                                   ? s.UserQuote.Quote.Content
                                   : s.UserQuote.CustomContent,
                    QuoteAuthor = s.UserQuote.QuoteId != null
                                  ? s.UserQuote.Quote.Author
                                  : s.UserQuote.CustomAuthor,
                    // Format Timespans to Strings for the JSON response
                    StartTime = s.DailyStartTime.ToString(@"hh\:mm"),
                    EndTime = s.DailyEndTime.ToString(@"hh\:mm"),
                    // Map the child collection of days to a simple list of ints
                    DaysOfWeek = s.Days.Select(d => d.DayOfWeek).ToList()
                })
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }


        public async Task<bool> RemoveScheduleAsync(Guid scheduleId)
        {
            var item = await _context.QuoteSchedules.FindAsync(scheduleId);
            if (item == null) return false;
            _context.QuoteSchedules.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
