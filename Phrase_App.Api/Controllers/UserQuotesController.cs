using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phrase_App.Api.Extensions;
using Phrase_App.Core.DTOs.Request;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserQuotesController : ControllerBase
{
    private readonly IUserQuoteService _userQuoteService;

    public UserQuotesController(IUserQuoteService userQuoteService)
    {
        _userQuoteService = userQuoteService;
    }

    [HttpGet("quotes")]
    public async Task<IActionResult> Get([FromQuery] bool onlyFavorites = false, bool isListingRequest = false)
    {
        var result = await _userQuoteService.GetUserQuotesAsync(User.GetUserId(), onlyFavorites, isListingRequest);
        return Ok(result);
    }

    [HttpPost("system")]
    public async Task<IActionResult> CreateSystem(AddSystemQuoteDto dto)
    {
        var res = await _userQuoteService.AddSystemQuoteAsync(dto, User.GetUserId());
        if (!res.Success) return BadRequest(new { success = false, res.Message });

        return Ok(new { success = true, message = res.Message });
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> CreateUserQuotesBulk([FromBody] List<AddCustomQuoteDto> dtos)
    {
        var res = await _userQuoteService.AddBulkQuotesAsync(dtos, User.GetUserId());
        if (!res.Success) return BadRequest(new { success = false, res.Message });

        return Ok(new { success = true, message = res.Message });
    }

    [HttpPost("toggle-favorite")]
    public async Task<IActionResult> Toggle(Guid quoteId)
    {
        var success = await _userQuoteService.ToggleFavoriteSystemQuoteAsync(quoteId);
        if (!success) return BadRequest(new { success = false, message = "Failed" });

        return Ok(new { success = true, message = "Successfull" });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserQuoteAsync(Guid id)
    {
        var success = await _userQuoteService.DeleteUserQuoteAsync(User.GetUserId(), id);
        if (!success) return BadRequest(new { success = false, message = "Failed" });

        return Ok(new { success = true, message = "Successfull" });
    }
}