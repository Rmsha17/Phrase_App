using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phrase_App.Core.DTOs.Request;
using Phrase_App.Core.DTOs.Response;
using Phrase_App.Core.Models;

namespace Phrase_App.Infrastructure.Services
{
    public class UserQuoteService : IUserQuoteService
    {
        private readonly PhraseDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserQuoteService(PhraseDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserQuoteResponseDto>> GetUserQuotesAsync(Guid? userId, bool onlyFavorites, bool isListingRequest)
        {
            var query = _context.UserQuotes
                                .Include(uq => uq.Quote)
                                .Where(uq => uq.UserId == userId && uq.IsActive);

            if (!isListingRequest)
            {
                var userQuotesIds = _context.QuoteSchedules.Where(qs => qs.UserId == userId).Select(sq => sq.UserQuoteId).ToList();
                query = query.Where(uq => !userQuotesIds.Contains(uq.Id));
            }

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

        public async Task<Response> AddSystemQuoteAsync(AddSystemQuoteDto dto, Guid? userId)
        {
            if (userId == Guid.Empty)
                return Response.FailResponse(StaticDetails.userLoginRequired);

            var date = DateTime.UtcNow;
            var user = await GetUser(userId);

            var existingUserQuotes = await _context.UserQuotes
                                                   .Where(uq => uq.UserId == userId && uq.IsActive)
                                                   .ToListAsync();

            if (user.IsPremium || existingUserQuotes.Count < 5)
            {
                if (dto is null || dto.QuoteId == Guid.Empty)
                    return Response.FailResponse("QuoteId is required.");

                var existing = await _context.UserQuotes
                                             .FirstOrDefaultAsync(uq => uq.UserId == userId && uq.QuoteId == dto.QuoteId);

                if (existing != null)
                    return Response.FailResponse("Quote already added to your collection.");

                var userQuote = new UserQuote
                {
                    Id = Guid.NewGuid(),
                    UserId = userId.Value,
                    QuoteId = dto.QuoteId,
                    IsFavorite = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserQuotes.Add(userQuote);
                await _context.SaveChangesAsync();

                return Response.SuccessResponse("Quote added.");
            }
            else
                return Response.FailResponse("You already reached  free limit of your account.");

        }

        public async Task<Response> AddBulkQuotesAsync(List<AddCustomQuoteDto> dtos, Guid? userId)
        {
            if (userId == Guid.Empty)
                return Response.FailResponse(StaticDetails.userLoginRequired);

            if (dtos == null || !dtos.Any())
                return new Response { Success = false, Message = "No quotes provided." };

            var userQuotes = dtos.Select(dto => new UserQuote
            {
                Id = Guid.NewGuid(),
                UserId = userId.Value,
                CustomContent = dto.Content,
                CustomAuthor = string.IsNullOrWhiteSpace(dto.Author) ? "Me" : dto.Author,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            // Bulk Insert
            await _context.UserQuotes.AddRangeAsync(userQuotes);
            await _context.SaveChangesAsync();

            return new Response
            {
                Success = true,
                Message = $"{userQuotes.Count} affirmations saved successfully!"
            };
        }

        public async Task<bool> ToggleFavoriteSystemQuoteAsync(Guid quoteId)
        {
            var existing = await _context.UserQuotes.FirstOrDefaultAsync(x => x.Id == quoteId);
            existing.IsFavorite = !existing.IsFavorite;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserQuoteAsync(Guid? userId, Guid id)
        {
            var quote = await _context.UserQuotes.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (quote == null) return false;

            _context.UserQuotes.Remove(quote);
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<ApplicationUser?> GetUser(Guid? userId)
        {
            var idString = userId.ToString();
            var user = await _userManager.FindByIdAsync(idString);
            return user;
        }
    }
}
