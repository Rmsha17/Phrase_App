namespace Phrase_App.Core.DTOs.Request
{
    public class SelectActiveQuotesDto
    {
        public List<Guid> UserQuoteIds { get; set; } = new();
    }
}