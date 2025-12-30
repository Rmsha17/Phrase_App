namespace Phrase_App.Core.DTOs.Request
{
    public class WeeklyScheduleRequestDto
    {
        public Guid UserId { get; set; }
        public Guid UserQuoteId { get; set; }
        public List<int> DaysOfWeek { get; set; } = new(); // [0,1,2,3,4,5,6]
        public string StartTime { get; set; } // "09:00"
        public string EndTime { get; set; }   // "17:00"
    }
}
