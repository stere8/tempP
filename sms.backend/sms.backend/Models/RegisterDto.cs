namespace sms.backend.Models
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // e.g., "Admin", "Teacher", "Student", "Parent"
    }
}
