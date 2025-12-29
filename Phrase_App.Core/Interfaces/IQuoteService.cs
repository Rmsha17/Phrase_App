using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Models;

namespace Phrase_App.Core.Interfaces
{
    public interface IQuoteService
    {
        Task<QuoteResponse> AddQuoteAsync(CreateQuoteRequest request);
        Task<List<QuoteResponse>> GetQuotesByCategoryIdAsync(Guid categoryId);
    }
}
