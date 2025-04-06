namespace sms.backend.Models
{
    public class Recipient
    {
        public string Type { get; set; } // "Student" or "Class"
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
