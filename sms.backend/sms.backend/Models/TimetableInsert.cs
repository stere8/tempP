namespace sms.backend.Models
{
    public class TimetableInsert
    {
        public int ClassId { get; set; }
        public int LessonId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
