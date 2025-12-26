// Phrase_App.Core/Interfaces/ICategoryService.cs
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllActiveCategoriesAsync();
    Task<CategoryResponse?> GetByIdAsync(Guid id);
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
    Task<bool> UpdateAsync(Guid id, UpdateCategoryRequest request);
    Task<bool> DeleteAsync(Guid id);
}