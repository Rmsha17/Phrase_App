// Phrase_App.Api/Controllers/CategoriesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.Constants;
using Phrase_App.Core.DTOs.Request;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService) => _categoryService = categoryService;
    
    [HttpGet("meta-data")]
    public IActionResult GetMetaData()
    {
        return Ok(new
        {
            AllowedIcons = CategoryDefaults.Icons.Keys,
            AllowedColors = CategoryDefaults.Colors
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActiveCategoriesAsync() => Ok(await _categoryService.GetAllActiveCategoriesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequest request)
    {
        var success = await _categoryService.UpdateAsync(id, request);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _categoryService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}