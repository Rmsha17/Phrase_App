namespace Phrase_App.Core.Models
{
    public class ScheduledDay
    {
        public int Id { get; set; }
        public Guid ScheduleId { get; set; }
        public int DayOfWeek { get; set; } // 0-6
    }
}
