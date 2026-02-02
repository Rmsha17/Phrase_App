using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Admin.Models;
using System.Diagnostics;

namespace Phrase_App.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhraseDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, PhraseDbContext context = null)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Block 1: Total Users
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            // Block 2: Total Quotes 
            ViewBag.TotalQuotes = await _context.Quotes.CountAsync();

            // Block 3: Active Engine Hooks (Schedules)
            ViewBag.ActiveSchedules = await _context.QuoteSchedules.CountAsync();

            // Chart Data: Schedule Density (Same logic as report for consistency)
            var allSchedules = await _context.QuoteSchedules.ToListAsync();
            ViewBag.Morning = allSchedules.Count(s => s.DailyStartTime.Hours >= 5 && s.DailyStartTime.Hours < 12);
            ViewBag.Afternoon = allSchedules.Count(s => s.DailyStartTime.Hours >= 12 && s.DailyStartTime.Hours < 17);
            ViewBag.Evening = allSchedules.Count(s => s.DailyStartTime.Hours >= 17 && s.DailyStartTime.Hours < 22);
            ViewBag.Night = allSchedules.Count(s => (s.DailyStartTime.Hours >= 22) || (s.DailyStartTime.Hours < 5));

            // Top Categories Logic
            ViewBag.TopCategories = await _context.Categories
                .Select(c => new { c.Name, Count = _context.Quotes.Count(q => c.Id == q.CategoryId) })
                .OrderByDescending(x => x.Count)
                .Take(4)
                .ToListAsync();
            // Static Metrics for Admin insight
            ViewBag.NewUsersToday = 5;
            ViewBag.ServerLoad = "24%";
            ViewBag.LastSync = DateTime.Now.ToString("HH:mm");

            // Top Categories with color assignments
            ViewBag.TopCategories = await _context.Categories
                .Select(c => new {
                    c.Name,
                    Count = _context.Quotes.Count(q => c.Id == q.CategoryId),
                    // Calculate percentage for progress bars
                    Percent = _context.Quotes.Count(q => c.Id == q.CategoryId) > 0 ? (_context.Quotes.Count(q => c.Id == q.CategoryId) * 100) / 50 : 0
                })
                .OrderByDescending(x => x.Count)
                .Take(4)
                .ToListAsync();

            // 1. Live Feed: Get last 3 quotes added
            ViewBag.RecentQuotes = await _context.Quotes
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new { q.Content, q.Author })
                .Take(3)
                .ToListAsync();

            // 2. System Health Metric (Static or semi-dynamic)
            ViewBag.EngineUptime = "99.98%";
            ViewBag.SystemEfficiency = 88; // Percentage

            // 3. Category Density (with progress bar math)
            var totalQuotes = await _context.Quotes.CountAsync();
            ViewBag.TopCategories = await _context.Categories
                .Select(c => new {
                    c.Name,
                    Count = _context.Quotes.Count(q => c.Id == q.CategoryId),
                    Percent = totalQuotes > 0 ? (_context.Quotes.Count(q => c.Id == q.CategoryId) * 100) / totalQuotes : 0
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
