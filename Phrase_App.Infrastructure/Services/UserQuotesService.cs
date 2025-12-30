using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;

namespace Phrase_App.Infrastructure.Services
{
    public class UserQuoteService : IUserQuoteService
    {
        private readonly PhraseDbContext _context;

        public UserQuoteService(PhraseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserQuoteResponseDto>> GetUserQuotesAsync(Guid userId, bool onlyFavorites)
        {
            var query = _context.UserQuotes
                .Include(uq => uq.Quote)
                .Where(uq => uq.UserId == userId);

            if (onlyFavorites)
                query = query.Where(uq => uq.IsFavorite);

            return await query.Select(uq => new UserQuoteResponseDto
            {
                Id = uq.Id,
                Content = uq.QuoteId != null ? uq.Quote.Content : uq.CustomContent,
                Author = uq.QuoteId != null ? uq.Quote.Author : uq.CustomAuthor,
                IsFavorite = uq.IsFavorite,
                IsCustom = uq.QuoteId == null,
                CreatedAt = uq.CreatedAt
            }).OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<bool> AddCustomQuoteAsync(AddCustomQuoteDto dto)
        {
            var userQuote = new UserQuote
            {
                UserId = dto.UserId,
                CustomContent = dto.Content,
                CustomAuthor = dto.Author,
                IsFavorite = true // Default to favorite for custom entries
            };

            _context.UserQuotes.Add(userQuote);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ToggleFavoriteSystemQuoteAsync(Guid userId, Guid quoteId)
        {
            var existing = await _context.UserQuotes
                .FirstOrDefaultAsync(x => x.UserId == userId && x.QuoteId == quoteId);

            if (existing != null)
            {
                existing.IsFavorite = !existing.IsFavorite;
            }
            else
            {
                _context.UserQuotes.Add(new UserQuote
                {
                    UserId = userId,
                    QuoteId = quoteId,
                    IsFavorite = true
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserQuoteAsync(Guid id, Guid userId)
        {
            var quote = await _context.UserQuotes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (quote == null) return false;

            _context.UserQuotes.Remove(quote);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
