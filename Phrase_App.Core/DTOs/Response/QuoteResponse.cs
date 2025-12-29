namespace Phrase_App.Core.DTOs.Response
{
    public record QuoteResponse(Guid Id, string Content, string? Author, Guid CategoryId);
}
