using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface IUserQuoteService
{
    Task<IEnumerable<UserQuoteResponseDto>> GetUserQuotesAsync(Guid userId, bool onlyFavorites);
    Task<bool> AddCustomQuoteAsync(AddCustomQuoteDto dto);
    Task<bool> ToggleFavoriteSystemQuoteAsync(Guid userId, Guid quoteId);
    Task<bool> DeleteUserQuoteAsync(Guid id, Guid userId);
}