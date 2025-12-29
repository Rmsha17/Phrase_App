// MyApp.Infrastructure/Services/QuoteService.cs
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Interfaces;
using Phrase_App.Core.Models;

public class QuoteService : IQuoteService
{
    private readonly PhraseDbContext _context;

    public QuoteService(PhraseDbContext context) => _context = context;

    public async Task<QuoteResponse> AddQuoteAsync(CreateQuoteRequest request)
    {
        var quote = new Quote
        {
            Content = request.Content,
            Author = request.Author,
            CategoryId = request.CategoryId
        };

        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        return new QuoteResponse(quote.Id, quote.Content, quote.Author, quote.CategoryId);
    }

    public async Task<List<QuoteResponse>> GetQuotesByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Quotes
            .Where(q => q.CategoryId == categoryId)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new QuoteResponse(q.Id, q.Content, q.Author, q.CategoryId))
            .ToListAsync();
    }
}