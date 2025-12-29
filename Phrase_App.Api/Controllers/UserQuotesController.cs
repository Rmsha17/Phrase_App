using Microsoft.AspNetCore.Mvc;
using Phrase_App.Core.DTOs.Request;

[ApiController]
[Route("api/[controller]")]
public class UserQuotesController : ControllerBase
{
    private readonly IUserQuoteService _userQuoteService;

    public UserQuotesController(IUserQuoteService userQuoteService)
    {
        _userQuoteService = userQuoteService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(Guid userId, [FromQuery] bool onlyFavorites = false)
    {
        var result = await _userQuoteService.GetUserQuotesAsync(userId, onlyFavorites);
        return Ok(result);
    }

    [HttpPost("custom")]
    public async Task<IActionResult> CreateCustom(AddCustomQuoteDto dto)
    {
        var success = await _userQuoteService.AddCustomQuoteAsync(dto);
        return success ? Ok() : BadRequest("Could not save quote.");
    }

    [HttpPost("toggle-favorite")]
    public async Task<IActionResult> Toggle(Guid userId, Guid quoteId)
    {
        var success = await _userQuoteService.ToggleFavoriteSystemQuoteAsync(userId, quoteId);
        return success ? Ok() : BadRequest();
    }
}