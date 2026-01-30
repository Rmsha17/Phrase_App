namespace Phrase_App.Core.DTOs.Response
{
    public class UserQuoteResponseDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsCustom { get; set; }
        public DateTime CreatedAt { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
