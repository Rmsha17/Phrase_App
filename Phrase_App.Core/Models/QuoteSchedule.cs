namespace Phrase_App.Core.Models
{
    public class QuoteSchedule
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid UserQuoteId { get; set; }
        public UserQuote UserQuote { get; set; }

        public TimeSpan DailyStartTime { get; set; }
        public TimeSpan DailyEndTime { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property for the days
        public List<ScheduledDay> Days { get; set; } = new();
    }
}
