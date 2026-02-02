using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.Constants;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Models;

namespace Phrase_App.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly PhraseDbContext _context;

        public CategoryService(PhraseDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;
            // Get count of active phrases/quotes for this category
            var quoteCount = await _context.Quotes.CountAsync(p => p.CategoryId == category.Id);

            return new CategoryResponse(category.Id, category.Name, category.IconKey, category.ColorHex, quoteCount, category.CreatedDate);
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            // Validate Icon
            if (!CategoryDefaults.Icons.ContainsKey(request.IconKey))
                throw new ArgumentException("Invalid Icon Key provided.");

            // Validate Color
            if (!CategoryDefaults.Colors.Contains(request.ColorHex))
                throw new ArgumentException("Invalid Color Hex provided.");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IconKey = request.IconKey,
                ColorHex = request.ColorHex,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponse(category.Id, category.Name, category.IconKey, category.ColorHex, 0, category.CreatedDate);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            // Validate Icon
            if (!CategoryDefaults.Icons.ContainsKey(request.IconKey))
                throw new ArgumentException("Invalid Icon Key provided.");

            // Validate Color
            if (!CategoryDefaults.Colors.Contains(request.ColorHex))
                throw new ArgumentException("Invalid Color Hex provided.");

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.Name = request.Name;
            category.IconKey = request.IconKey;
            category.ColorHex = request.ColorHex;
            category.IsActive = request.IsActive;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            // Option A: Hard Delete (removes from DB)
            // _context.Categories.Remove(category);

            // Option B: Soft Delete (better if you have Phrases linked to it)
            category.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllActiveCategoriesAsync()
        {
            return await _context.Categories
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new CategoryResponse(x.Id, x.Name, x.IconKey, x.ColorHex,
                                    _context.Quotes.Count(p => p.CategoryId == x.Id), x.CreatedDate))
                .ToListAsync();
        }
    }
}
