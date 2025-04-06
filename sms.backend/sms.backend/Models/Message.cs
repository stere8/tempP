namespace sms.backend.Models
{
    public enum MessageTargetType
    {
        Student,
        Class
    }
    public class Message
    {
        public int Id { get; set; }
        public string SenderRole { get; set; } // "Admin" or "Teacher"
        public MessageTargetType TargetType { get; set; }
        public int? TargetId { get; set; } // Use when TargetType is Student or Class
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
