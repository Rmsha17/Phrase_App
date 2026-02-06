using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.Constants;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;

namespace Phrase_App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly PhraseDbContext _context;

        public CategoriesController(ICategoryService categoryService, PhraseDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        // GET: Admin/Categories
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllActiveCategoriesAsync();
            return View(categories.OrderByDescending(c => c.Id));
        }

        // GET: Admin/Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
            ViewBag.AllowedColors = CategoryDefaults.Colors;
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                return View(request);
            }

            try
            {
                await _categoryService.CreateAsync(request);
                TempData["Success"] = $"Category '{request.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["Error"] = ex.Message;
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred.";
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                return View(request);
            }
        }

        // GET: Admin/Categories/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
            ViewBag.AllowedColors = CategoryDefaults.Colors;
            ViewBag.CategoryId = entity.Id;

            var vm = new UpdateCategoryRequest(entity.Id, entity.Name, entity.IconKey, entity.ColorHex, entity.IsActive);
            return View(vm);
        }

        // POST: Admin/Categories/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCategoryRequest request)
        {
            if (id == Guid.Empty) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                ViewBag.CategoryId = id;
                return View(request);
            }

            try
            {
                var updated = await _categoryService.UpdateAsync(id, request);
                if (!updated)
                {
                    TempData["Error"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                ViewBag.CategoryId = id;
                return View(request);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred.";
                ViewBag.AllowedIcons = CategoryDefaults.Icons.Keys;
                ViewBag.AllowedColors = CategoryDefaults.Colors;
                ViewBag.CategoryId = id;
                return View(request);
            }
        }

        // POST: Admin/Categories/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    TempData["Error"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                var success = await _categoryService.DeleteAsync(id);
                if (!success)
                {
                    TempData["Error"] = "Failed to delete category.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Category deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/Categories/MetaData (optional AJAX)
        [HttpGet]
        public IActionResult MetaData()
            => Json(new { AllowedIcons = CategoryDefaults.Icons.Keys, AllowedColors = CategoryDefaults.Colors });
    }
}