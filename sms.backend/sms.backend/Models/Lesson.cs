namespace sms.backend.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public int GradeLevel { get; set; }

    }
}