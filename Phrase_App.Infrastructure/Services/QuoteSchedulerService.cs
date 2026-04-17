using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Models;

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

        public async Task<List<UserQuoteResponseDto>> GetCurrentScheduledQuoteAsync(Guid? userId)
        {
            var now = DateTime.Now;
            int currentDay = (int)now.DayOfWeek;
            TimeSpan currentTimeOfDay = now.TimeOfDay;

            var schedules = await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId && s.IsActive &&
                          ((s.DailyStartTime < s.DailyEndTime && currentTimeOfDay >= s.DailyStartTime && currentTimeOfDay <= s.DailyEndTime) ||
                           (s.DailyStartTime >= s.DailyEndTime && (currentTimeOfDay >= s.DailyStartTime || currentTimeOfDay <= s.DailyEndTime)) &&
                            s.Days.Any(d => d.DayOfWeek == currentDay)))
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

        public async Task<List<UserQuoteResponseDto>> GetActiveScheduledQuoteAsync(Guid? userId)
        {
            var now = DateTime.Now;
            int currentDay = (int)now.DayOfWeek;

            var schedules = await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId && s.IsActive &&
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
                IsCustom = s.UserQuote.QuoteId == null,
                StartTime = s.DailyStartTime,
                EndTime = s.DailyEndTime
            }).ToList();
        }

        public async Task<IEnumerable<ScheduleResponseDto>> GetUserSchedulesAsync(Guid? userId)
        {
            return await _context.QuoteSchedules
                .Include(s => s.Days)
                .Include(s => s.UserQuote)
                    .ThenInclude(uq => uq.Quote)
                .Where(s => s.UserId == userId && s.IsActive)
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

        public async Task<bool> UpdateWeeklyAsync(Guid scheduleId, UpdateWeeklyScheduleDto dto, Guid? userId)
        {
            var schedule = await _context.QuoteSchedules
                .Include(s => s.Days)
                .FirstOrDefaultAsync(s => s.Id == scheduleId && s.UserId == userId.Value);

            if (schedule == null) return false;

            // Update time
            schedule.DailyStartTime = TimeSpan.Parse(dto.StartTime);
            schedule.DailyEndTime = TimeSpan.Parse(dto.EndTime);

            // Remove old days and replace with new ones
            _context.ScheduledDays.RemoveRange(schedule.Days);
            schedule.Days = dto.DaysOfWeek.Select(day => new ScheduledDay
            {
                DayOfWeek = day,
            }).ToList();

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Replaces all existing schedules with 24/7 all-days schedules for each selected quote.
        /// The user never sees any scheduling UI — this is done automatically behind the scenes.
        /// </summary>
        public async Task<bool> SelectActiveQuotesAsync(List<Guid> userQuoteIds, Guid userId)
        {
            // Get all existing schedules for this user
            var existing = await _context.QuoteSchedules
                .Include(s => s.Days)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            var alreadyScheduledQuoteIds = existing.Select(s => s.UserQuoteId).ToList();

            // 1. Remove schedules for quotes that are NO LONGER selected
            var toRemove = existing
                .Where(s => !userQuoteIds.Contains(s.UserQuoteId))
                .ToList();

            foreach (var s in toRemove)
                _context.ScheduledDays.RemoveRange(s.Days);

            _context.QuoteSchedules.RemoveRange(toRemove);

            // 2. Add schedules only for quotes that are newly selected (not already in DB)
            var allDays = new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // Sun–Sat

            var toAdd = userQuoteIds
                .Where(id => !alreadyScheduledQuoteIds.Contains(id))
                .ToList();

            foreach (var userQuoteId in toAdd)
            {
                var schedule = new QuoteSchedule
                {
                    UserId         = userId,
                    UserQuoteId    = userQuoteId,
                    DailyStartTime = TimeSpan.Zero,             // 00:00
                    DailyEndTime   = new TimeSpan(23, 59, 59),  // 23:59
                    IsActive       = true,
                    CreatedAt      = DateTime.UtcNow,
                    Days           = allDays.Select(d => new ScheduledDay { DayOfWeek = d }).ToList()
                };
                _context.QuoteSchedules.Add(schedule);
            }

            return await _context.SaveChangesAsync() >= 0;
        }

        /// <summary>
        /// Returns all quotes for this user, with IsSelected=true if they currently have an active schedule.
        /// </summary>
        public async Task<List<UserQuoteWithSelectionDto>> GetUserQuotesWithSelectionAsync(Guid userId)
        {
            // Get all UserQuote IDs that have an active schedule
            var scheduledQuoteIds = await _context.QuoteSchedules
                .Where(s => s.UserId == userId && s.IsActive)
                .Select(s => s.UserQuoteId)
                .ToListAsync();

            var userQuotes = await _context.UserQuotes
                .Include(uq => uq.Quote)
                .Where(uq => uq.UserId == userId)
                .OrderByDescending(uq => uq.CreatedAt)
                .ToListAsync();

            return userQuotes.Select(uq => new UserQuoteWithSelectionDto
            {
                Id         = uq.Id,
                Content    = uq.QuoteId != null ? uq.Quote.Content : uq.CustomContent,
                Author     = uq.QuoteId != null ? uq.Quote.Author  : uq.CustomAuthor,
                IsFavorite = uq.IsFavorite,
                IsCustom   = uq.QuoteId == null,
                IsSelected = scheduledQuoteIds.Contains(uq.Id)
            }).ToList();
        }
    }
}