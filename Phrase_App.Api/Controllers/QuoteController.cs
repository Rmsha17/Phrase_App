using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class QuotesController : ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuotesController(IQuoteService quoteService) => _quoteService = quoteService;

    [HttpPost]
    [Authorize(Roles = "Admin")] // Only admins should add quotes
    public async Task<IActionResult> Add(CreateQuoteRequest request)
    {
        var result = await _quoteService.AddQuoteAsync(request);
        return Ok(result);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId)
    {
        var quotes = await _quoteService.GetQuotesByCategoryIdAsync(categoryId);
        return Ok(quotes);
    }
}