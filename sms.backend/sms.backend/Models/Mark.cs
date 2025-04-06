namespace sms.backend.Models
{
    public class Mark
    {
        public int MarkId { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public decimal MarkValue { get; set; }
        public DateTime Date { get; set; }
    }
}