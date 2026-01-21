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

        public async Task<bool> ScheduleWeeklyAsync(WeeklyScheduleRequestDto dto, Guid? userId)
        {
            var schedule = new QuoteSchedule
            {
                UserId = userId.Value,
                UserQuoteId = dto.UserQuoteId,
                DailyStartTime = TimeSpan.Parse(dto.StartTime), // Expects "HH:mm"
                DailyEndTime = TimeSpan.Parse(dto.EndTime),
                Days = dto.DaysOfWeek.Select(day => new ScheduledDay
                {
                    DayOfWeek = day,
                }).ToList(),
                CreatedAt = DateTime.UtcNow
            };

            _context.QuoteSchedules.Add(schedule);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserQuoteResponseDto>> GetActiveScheduledQuoteAsync(Guid? userId)
        {
            var now = DateTime.Now;
            int currentDay = (int)now.DayOfWeek;

            var schedules = await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId &&
                            s.Days.Any(d => d.DayOfWeek == currentDay)) // Only filtering by Today's Day
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return schedules.Select(s => new UserQuoteResponseDto
            {
                Id = s.UserQuote.Id,
                Content = s.UserQuote.QuoteId != null
                          ? s.UserQuote.Quote.Content
                          : s.UserQuote.CustomContent,
                Author = s.UserQuote.QuoteId != null
                         ? s.UserQuote.Quote.Author
                         : s.UserQuote.CustomAuthor,
                IsFavorite = s.UserQuote.IsFavorite,
                IsCustom = s.UserQuote.QuoteId == null
            }).ToList();
        }

        public async Task<IEnumerable<ScheduleResponseDto>> GetUserSchedulesAsync(Guid? userId)
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
