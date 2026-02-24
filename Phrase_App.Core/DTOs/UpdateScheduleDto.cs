namespace Phrase_App.Core.DTOs
{
    public class UpdateWeeklyScheduleDto
    {
        public List<int> DaysOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
