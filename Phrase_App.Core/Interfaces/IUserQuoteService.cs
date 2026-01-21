using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

public interface IUserQuoteService
{
    Task<Response> AddSystemQuoteAsync(AddSystemQuoteDto dto, Guid? userId);
    Task<IEnumerable<UserQuoteResponseDto>> GetUserQuotesAsync(Guid? userId, bool onlyFavorites, bool isListingRequest);
    Task<bool> ToggleFavoriteSystemQuoteAsync(Guid quoteId);
    Task<bool> DeleteUserQuoteAsync(Guid? id, Guid userId);
    Task<Response> AddBulkQuotesAsync(List<AddCustomQuoteDto> dtos, Guid? userId);
}