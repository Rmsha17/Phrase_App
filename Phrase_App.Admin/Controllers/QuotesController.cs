using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Constants;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Models;
using System.Text;

namespace Phrase_App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuotesController : Controller
    {
        private readonly PhraseDbContext _context;

        public QuotesController(PhraseDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Quotes
        [HttpGet]
        public async Task<IActionResult> Index(Guid? categoryId)
        {
            var query = _context.Quotes.Include(q => q.Category).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(q => q.CategoryId == categoryId.Value);

            var total = await query.CountAsync();
            var quotes = await query
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();

            return View(quotes);
        }

        // GET: Admin/Quotes/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        // POST: Admin/Quotes/Create (single quote)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Author,CategoryId")] Quote quote)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View(quote);
            }

            try
            {
                quote.Id = Guid.NewGuid();
                quote.CreatedAt = DateTime.UtcNow;
                _context.Quotes.Add(quote);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Quote created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while creating the quote.";
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View(quote);
            }
        }

        // GET: Admin/Quotes/BulkUpload
        [HttpGet]
        public async Task<IActionResult> BulkUpload()
        {
            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        // POST: Admin/Quotes/BulkUpload (file upload)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpload(IFormFile file, Guid categoryId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file.";
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View();
            }

            if (!file.FileName.EndsWith(".csv") && !file.FileName.EndsWith(".txt"))
            {
                TempData["Error"] = "Only CSV and TXT files are allowed.";
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View();
            }

            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                TempData["Error"] = "Selected category not found.";
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View();
            }

            var createdCount = 0;
            var errorLines = new List<string>();

            try
            {
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string? line;
                    int lineNumber = 0;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lineNumber++;

                        // Skip empty lines and comments
                        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                            continue;

                        var parts = line.Split('|');

                        if (parts.Length < 1)
                        {
                            errorLines.Add($"Line {lineNumber}: Invalid format (no quote content).");
                            continue;
                        }

                        var content = parts[0]?.Trim();
                        var author = parts.Length > 1 ? parts[1]?.Trim() : null;

                        if (string.IsNullOrWhiteSpace(content))
                        {
                            errorLines.Add($"Line {lineNumber}: Quote content cannot be empty.");
                            continue;
                        }

                        try
                        {
                            var quote = new Quote
                            {
                                Id = Guid.NewGuid(),
                                Content = content,
                                Author = string.IsNullOrWhiteSpace(author) ? "Unknown" : author,
                                CategoryId = category.Id,
                                CreatedAt = DateTime.UtcNow
                            };

                            _context.Quotes.Add(quote);
                            createdCount++;
                        }
                        catch (Exception ex)
                        {
                            errorLines.Add($"Line {lineNumber}: {ex.Message}");
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                var message = $"Successfully imported {createdCount} quotes.";
                if (errorLines.Count > 0)
                    message += $" {errorLines.Count} lines had errors.";

                TempData["Success"] = message;

                if (errorLines.Count > 0)
                    TempData["Errors"] = string.Join("\n", errorLines.Take(10));

                return RedirectToAction(nameof(Index), new { categoryId = categoryId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during bulk upload: " + ex.Message;
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View();
            }
        }

        // GET: Admin/Quotes/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var quote = await _context.Quotes.Include(q => q.Category).FirstOrDefaultAsync(q => q.Id == id);
            if (quote == null)
            {
                TempData["Error"] = "Quote not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
            return View(quote);
        }

        // POST: Admin/Quotes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Content,Author,CategoryId")] Quote quote)
        {
            if (id != quote.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View(quote);
            }

            try
            {
                var existing = await _context.Quotes.FindAsync(id);
                if (existing == null)
                {
                    TempData["Error"] = "Quote not found.";
                    return RedirectToAction(nameof(Index));
                }

                existing.Content = quote.Content;
                existing.Author = quote.Author;
                existing.CategoryId = quote.CategoryId;

                _context.Quotes.Update(existing);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Quote updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the quote.";
                ViewBag.Categories = await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
                return View(quote);
            }
        }

        // POST: Admin/Quotes/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var quote = await _context.Quotes.FindAsync(id);
                if (quote == null)
                {
                    TempData["Error"] = "Quote not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Quote deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the quote.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}