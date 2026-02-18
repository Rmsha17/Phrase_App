using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs;
using Phrase_App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phrase_App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly PhraseDbContext _context;

        public ReportsController(PhraseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            // Get user stats in one go
            var userStats = await _context.Users
                .Select(u => new { u.IsPremium, u.CreatedAt })
                .ToListAsync();

            var totalUsers = userStats.Count;
            var premiumUsers = userStats.Count(u => u.IsPremium);

            // Active users logic remains similar but optimized
            var activeUsersCount = await _context.UserQuotes
                .Where(uq => uq.CreatedAt >= sevenDaysAgo)
                .Select(uq => uq.UserId)
                .Distinct()
                .CountAsync();

            var model = new DashboardReportViewModel
            {
                TotalUsers = totalUsers,
                PremiumUsers = premiumUsers,
                FreeUsers = totalUsers - premiumUsers,
                TotalQuotes = await _context.Quotes.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(c => c.IsActive),
                RecentSignups = userStats.Count(u => u.CreatedAt >= sevenDaysAgo),
                ActiveUsersLast7Days = activeUsersCount,
                GeneratedAt = DateTime.UtcNow
            };

            return View(model);
        }

        // GET: Admin/Reports/Subscriptions
        [HttpGet]
        public async Task<IActionResult> Subscriptions()
        {
            var now = DateTime.UtcNow;
            var thirtyDaysFromNow = now.AddDays(30);

            // Premium users breakdown
            var monthlySubscribers = await _context.Users
                .Where(u => u.IsPremium && u.SubscriptionType == "Monthly")
                .CountAsync();

            var yearlySubscribers = await _context.Users
                .Where(u => u.IsPremium && u.SubscriptionType == "Yearly")
                .CountAsync();

            // Expiring soon (within 30 days)
            var expiringUsers = await _context.Users
                .Where(u => u.IsPremium && u.PremiumExpiryDate.HasValue &&
                           u.PremiumExpiryDate.Value > now &&
                           u.PremiumExpiryDate.Value <= thirtyDaysFromNow)
                .Select(u => new SubscriptionExpiryDto
                {
                    UserId = u.Id,
                    UserEmail = u.Email,
                    SubscriptionType = u.SubscriptionType,
                    ExpiryDate = u.PremiumExpiryDate.Value
                })
                .ToListAsync();

            // Already expired
            var expiredUsers = await _context.Users
                .CountAsync(u => u.IsPremium && u.PremiumExpiryDate.HasValue &&
                                 u.PremiumExpiryDate.Value <= now);

            var model = new SubscriptionReportViewModel
            {
                MonthlySubscribers = monthlySubscribers,
                YearlySubscribers = yearlySubscribers,
                ExpiringWithin30Days = expiringUsers.Count,
                ExpiredSubscriptions = expiredUsers,
                ExpiringUsers = expiringUsers,
                GeneratedAt = DateTime.UtcNow
            };

            return View(model);
        }

        // GET: Admin/Reports/ContentAnalytics
        [HttpGet]
        public async Task<IActionResult> ContentAnalytics()
        {
            // Quotes per category
            var quotesByCategory = await _context.Quotes
                .Include(q => q.Category)
                .GroupBy(q => q.Category!.Name)
                .Select(g => new QuoteByCategoryDto
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            // Top 10 most favorited quotes
            var topQuotes = await _context.UserQuotes
                .Where(uq => uq.IsFavorite && uq.QuoteId.HasValue)
                .GroupBy(uq => uq.QuoteId)
                .Select(g => new TopQuoteDto
                {
                    QuoteId = g.Key!.Value,
                    FavoriteCount = g.Count(),
                    QuoteText = _context.Quotes.Where(q => q.Id == g.Key).Select(q => q.Content).FirstOrDefault()
                })
                .OrderByDescending(x => x.FavoriteCount)
                .Take(10)
                .ToListAsync();

            // Quotes added per day (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var quotesPerDay = await _context.Quotes
                .Where(q => q.CreatedAt >= thirtyDaysAgo)
                .GroupBy(q => q.CreatedAt.Date)
                .Select(g => new QuotesPerDayDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var model = new ContentAnalyticsViewModel
            {
                QuotesByCategory = quotesByCategory,
                TopQuotes = topQuotes,
                QuotesPerDay = quotesPerDay,
                TotalQuotesLast30Days = quotesPerDay.Sum(x => x.Count),
                GeneratedAt = DateTime.UtcNow
            };

            return View(model);
        }

        // GET: Admin/Reports/UserEngagement
        [HttpGet]
        public async Task<IActionResult> UserEngagement()
        {
            var now = DateTime.UtcNow;
            var sevenDaysAgo = now.AddDays(-7);
            var thirtyDaysAgo = now.AddDays(-30);
            var ninetyDaysAgo = now.AddDays(-90);

            // Active users in different time periods
            var activeLast7 = await _context.UserQuotes
                .Where(uq => uq.CreatedAt >= sevenDaysAgo)
                .Select(uq => uq.UserId)
                .Distinct()
                .CountAsync();

            var activeLast30 = await _context.UserQuotes
                .Where(uq => uq.CreatedAt >= thirtyDaysAgo)
                .Select(uq => uq.UserId)
                .Distinct()
                .CountAsync();

            var activeLast90 = await _context.UserQuotes
                .Where(uq => uq.CreatedAt >= ninetyDaysAgo)
                .Select(uq => uq.UserId)
                .Distinct()
                .CountAsync();

            // User growth (daily for last 30 days)
            var userGrowth = await _context.Users
                .Where(u => u.CreatedAt >= thirtyDaysAgo)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new UserGrowthDto
                {
                    Date = g.Key,
                    NewUsers = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Dark mode adoption
            var darkModeEnabled = await _context.Users.CountAsync(u => u.DarkMode);
            var totalUsers = await _context.Users.CountAsync();
            var darkModeAdoption = totalUsers > 0 ? (darkModeEnabled * 100.0 / totalUsers) : 0;

            var model = new UserEngagementViewModel
            {
                ActiveUsersLast7Days = activeLast7,
                ActiveUsersLast30Days = activeLast30,
                ActiveUsersLast90Days = activeLast90,
                UserGrowth = userGrowth,
                DarkModeEnabledCount = darkModeEnabled,
                DarkModeAdoptionPercentage = darkModeAdoption,
                GeneratedAt = DateTime.UtcNow
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ErrorLogs(int page = 1, int pageSize = 50)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var query = _context.ErrorLogs.AsNoTracking(); // Performance boost for read-only

            var total = await query.CountAsync();
            var errors = await query
                .OrderByDescending(e => e.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Summary logic
            var errorsByType = await query
                .GroupBy(e => e.ErrorType ?? "Unknown")
                .Select(g => new ErrorSummaryDto { ErrorType = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var errorsByPlatform = await query
                .Where(e => e.Timestamp >= thirtyDaysAgo)
                .GroupBy(e => e.Platform ?? "Unknown")
                .Select(g => new ErrorPlatformDto { Platform = g.Key, Count = g.Count() })
                .ToListAsync();

            var model = new ErrorLogsViewModel
            {
                ErrorLogs = errors,
                Total = total,
                Page = page,
                PageSize = pageSize,
                ErrorsByType = errorsByType,
                ErrorsByPlatform = errorsByPlatform,
                GeneratedAt = DateTime.UtcNow
            };

            return View(model);
        }
        public async Task<IActionResult> ScheduleReport()
        {
            // ── 1. Day-of-week density ──────────────────────────────────────────
            // Load to memory FIRST to avoid EF translation errors on enum.ToString()
            var scheduledDayEnums = await _context.ScheduledDays
                .Select(d => d.DayOfWeek)   // DayOfWeek enum column
                .ToListAsync();

            // Count per day in memory
            var dayCountDict = scheduledDayEnums
                .GroupBy(d => d)
                .ToDictionary(g => g.Key, g => g.Count());

            // Always emit all 7 days in Mon→Sun order so the heatmap is never missing cells
            var orderedDays = new[]
            {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
        DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
    };

            var dayDistribution = orderedDays
                .Select(d => new DayDistributionDto
                {
                    Day = d.ToString(),
                    Count = dayCountDict.TryGetValue((int)d, out var c) ? c : 0
                })
                .ToList();


            // ── 2. High-frequency users ─────────────────────────────────────────
            // Step A: aggregate schedule counts per UserId in SQL
            var scheduleGroups = await _context.QuoteSchedules
                .GroupBy(s => s.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)   // fetch a few extra in case some user IDs don't resolve
                .ToListAsync();

            // Step B: resolve emails from Users table
            var userIdStrings = scheduleGroups
                .Select(x => x.UserId.ToString())
                .ToList();

            var userEmails = await _context.Users
                .Where(u => userIdStrings.Contains(u.Id))
                .Select(u => new { u.Id, u.Email })
                .ToListAsync();

            // Step C: join in memory using named type — avoids anonymous type boxing issues
            var powerUsers = scheduleGroups
                .Join(
                    userEmails,
                    sg => sg.UserId.ToString(),
                    ue => ue.Id,
                    (sg, ue) => new { UserEmail = ue.Email ?? "Unknown", Count = sg.Count }
                )
                .Take(5)
                .ToList();


            // ── 3. Time-window breakdown ────────────────────────────────────────
            var allSchedules = await _context.QuoteSchedules
                .AsNoTracking()
                .ToListAsync();

            int morning = allSchedules.Count(s => s.DailyStartTime.Hours >= 5 && s.DailyStartTime.Hours < 12);
            int afternoon = allSchedules.Count(s => s.DailyStartTime.Hours >= 12 && s.DailyStartTime.Hours < 17);
            int evening = allSchedules.Count(s => s.DailyStartTime.Hours >= 17 && s.DailyStartTime.Hours < 22);
            int night = allSchedules.Count(s => s.DailyStartTime.Hours >= 22 || s.DailyStartTime.Hours < 5);

            ViewBag.Morning = morning;
            ViewBag.Afternoon = afternoon;
            ViewBag.Evening = evening;
            ViewBag.Night = night;


            // ── 4. Dynamic insights ─────────────────────────────────────────────
            var peakDay = dayDistribution
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            ViewBag.PeakDay = peakDay?.Day ?? "N/A";

            var peakHour = allSchedules
                .GroupBy(s => s.DailyStartTime.Hours)
                .Select(g => new { Hour = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            ViewBag.PeakTimeSlot = peakHour != null
                ? $"{peakHour.Hour:D2}:00 – {(peakHour.Hour + 1) % 24:D2}:00"
                : "N/A";

            ViewBag.DayDistribution = dayDistribution;   // List<DayDistributionDto> — typed, safe
            ViewBag.PowerUsers = powerUsers;

            return View();
        }
    }

    // ViewModels
    public class DashboardReportViewModel
    {
        public int TotalUsers { get; set; }
        public int PremiumUsers { get; set; }
        public int FreeUsers { get; set; }
        public int TotalQuotes { get; set; }
        public int TotalCategories { get; set; }
        public int RecentSignups { get; set; }
        public int ActiveUsersLast7Days { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class SubscriptionReportViewModel
    {
        public int MonthlySubscribers { get; set; }
        public int YearlySubscribers { get; set; }
        public int ExpiringWithin30Days { get; set; }
        public int ExpiredSubscriptions { get; set; }
        public List<SubscriptionExpiryDto> ExpiringUsers { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public class SubscriptionExpiryDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string SubscriptionType { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }

    public class ContentAnalyticsViewModel
    {
        public List<QuoteByCategoryDto> QuotesByCategory { get; set; } = new();
        public List<TopQuoteDto> TopQuotes { get; set; } = new();
        public List<QuotesPerDayDto> QuotesPerDay { get; set; } = new();
        public int TotalQuotesLast30Days { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class QuoteByCategoryDto
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class TopQuoteDto
    {
        public Guid QuoteId { get; set; }
        public int FavoriteCount { get; set; }
        public string QuoteText { get; set; } = string.Empty;
    }

    public class QuotesPerDayDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class UserEngagementViewModel
    {
        public int ActiveUsersLast7Days { get; set; }
        public int ActiveUsersLast30Days { get; set; }
        public int ActiveUsersLast90Days { get; set; }
        public List<UserGrowthDto> UserGrowth { get; set; } = new();
        public int DarkModeEnabledCount { get; set; }
        public double DarkModeAdoptionPercentage { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class UserGrowthDto
    {
        public DateTime Date { get; set; }
        public int NewUsers { get; set; }
    }

    public class ErrorLogsViewModel
    {
        public List<ErrorLog> ErrorLogs { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ErrorSummaryDto> ErrorsByType { get; set; } = new();
        public List<ErrorPlatformDto> ErrorsByPlatform { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public class ErrorSummaryDto
    {
        public string ErrorType { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ErrorPlatformDto
    {
        public string Platform { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
