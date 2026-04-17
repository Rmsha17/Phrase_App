namespace Phrase_App.Core.DTOs.Response
{
    public class UserQuoteWithSelectionDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsCustom { get; set; }
        public bool IsSelected { get; set; }
    }
}