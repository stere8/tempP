namespace sms.backend.Views;

public class TimetableView
{
    public string DayOfWeek { get; set; }
    public string LessonName { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string TeachersName { get; set; }
}