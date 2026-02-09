using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Admin.Models;
using System.Diagnostics;

namespace Phrase_App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
            if (User.Identity?.IsAuthenticated == false)
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("Login", "Account");
            }

            var now = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayOfMonth.AddMonths(-1);

            // 1. TOTAL USERS & GROWTH RATE
            var totalUsers = await _context.Users.CountAsync();
            var usersThisMonth = await _context.Users.CountAsync(u => u.CreatedAt >= firstDayOfMonth);
            var usersLastMonth = await _context.Users.CountAsync(u => u.CreatedAt >= firstDayLastMonth && u.CreatedAt < firstDayOfMonth);

            double growthRate = 0;
            if (usersLastMonth > 0)
                growthRate = ((double)(usersThisMonth - usersLastMonth) / usersLastMonth) * 100;
            else if (usersThisMonth > 0)
                growthRate = 100;

            ViewBag.TotalUsers = totalUsers;
            ViewBag.UserGrowth = growthRate.ToString("F1");

            // 2. TOTAL QUOTES & TODAY'S ADDITIONS
            ViewBag.TotalQuotes = await _context.Quotes.CountAsync();
            ViewBag.QuotesToday = await _context.Quotes.CountAsync(q => q.CreatedAt.Date == now.Date);

            // 3. ENGINE UPTIME (Dynamic from Process)
            var uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            ViewBag.EngineUptime = uptime.Days > 0 ? $"{uptime.Days}d {uptime.Hours}h" : $"{uptime.Hours}h {uptime.Minutes}m";

            // 4. ACTIVE HOOKS & SYSTEM EFFICIENCY
            var activeHooks = await _context.QuoteSchedules.CountAsync();
            ViewBag.ActiveSchedules = activeHooks;

            // Efficiency calculation: % of quotes that are fully categorized and have authors
            var totalQ = await _context.Quotes.CountAsync();
            var completeQ = await _context.Quotes.CountAsync(q => !string.IsNullOrEmpty(q.Author) && q.CategoryId != null);
            ViewBag.SystemEfficiency = totalQ > 0 ? (completeQ * 100) / totalQ : 100;

            // 5. CHART DATA (24H CYCLE)
            var allSchedules = await _context.QuoteSchedules.ToListAsync();
            ViewBag.Morning = allSchedules.Count(s => s.DailyStartTime.Hours >= 5 && s.DailyStartTime.Hours < 12);
            ViewBag.Afternoon = allSchedules.Count(s => s.DailyStartTime.Hours >= 12 && s.DailyStartTime.Hours < 17);
            ViewBag.Evening = allSchedules.Count(s => s.DailyStartTime.Hours >= 17 && s.DailyStartTime.Hours < 22);
            ViewBag.Night = allSchedules.Count(s => (s.DailyStartTime.Hours >= 22) || (s.DailyStartTime.Hours < 5));

            // 6. TOP CATEGORIES (Coverage %)
            ViewBag.TopCategories = await _context.Categories
                .Select(c => new
                {
                    c.Name,
                    Count = _context.Quotes.Count(q => c.Id == q.CategoryId),
                    Percent = totalQ > 0 ? (_context.Quotes.Count(q => c.Id == q.CategoryId) * 100) / totalQ : 0
                })
                .OrderByDescending(x => x.Count).Take(5).ToListAsync();

            // 7. LIVE FEED
            ViewBag.RecentQuotes = await _context.Quotes
                .OrderByDescending(q => q.CreatedAt).Take(3)
                .Select(q => new { q.Content, q.Author }).ToListAsync();

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
