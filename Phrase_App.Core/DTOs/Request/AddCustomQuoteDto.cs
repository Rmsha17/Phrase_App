namespace Phrase_App.Core.DTOs.Request
{
    public class AddCustomQuoteDto
    {
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
