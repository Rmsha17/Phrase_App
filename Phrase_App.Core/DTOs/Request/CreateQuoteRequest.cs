namespace Phrase_App.Core.DTOs.Request
{
    public record CreateQuoteRequest(string Content, string? Author, Guid CategoryId);
}
