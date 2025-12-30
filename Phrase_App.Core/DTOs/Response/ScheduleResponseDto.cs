namespace Phrase_App.Core.DTOs.Response
{
    public class ScheduleResponseDto
    {
        public Guid Id { get; set; }

        // The specific Quote details being scheduled
        public Guid UserQuoteId { get; set; }
        public string? QuoteContent { get; set; }
        public string? QuoteAuthor { get; set; }

        // Time window for the day
        public string StartTime { get; set; } = string.Empty; // Format: "09:00"
        public string EndTime { get; set; } = string.Empty;   // Format: "17:00"

        // List of active days: 0 (Sun) through 6 (Sat)
        public List<int> DaysOfWeek { get; set; } = new();

        // Helper property to show a human-readable summary in Flutter
        public string TimeRangeSummary => $"{StartTime} - {EndTime}";
    }
}
