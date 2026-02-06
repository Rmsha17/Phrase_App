using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Models;

namespace Phrase_App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PhraseDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, PhraseDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Admin/Users
        [HttpGet]
        public async Task<IActionResult> Index(string? searchQuery)
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                usersQuery = usersQuery.Where(u =>
                    u.UserName!.Contains(searchQuery) ||
                    u.Email!.Contains(searchQuery) ||
                    u.FullName!.Contains(searchQuery)
                );
            }

            var total = await usersQuery.CountAsync();
            var users = await usersQuery
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.SearchQuery = searchQuery;

            return View(users);
        }

        // GET: Admin/Users/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;

            return View(user);
        }

        // GET: Admin/Users/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // POST: Admin/Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,DarkMode")] ApplicationUser model)
        {
            if (id != model.Id) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                user.FullName = model.FullName ?? user.FullName;
                user.DarkMode = model.DarkMode;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to update user.";
                    return View(model);
                }

                TempData["Success"] = "User updated successfully!";
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the user.";
                return View(model);
            }
        }

        // POST: Admin/Users/ToggleDarkMode/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDarkMode(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            user.DarkMode = !user.DarkMode;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Failed to toggle DarkMode." });
            }

            return Json(new { success = true, darkMode = user.DarkMode });
        }

        // POST: Admin/Users/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to delete user.";
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }

                TempData["Success"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the user.";
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }
        }
    }
}