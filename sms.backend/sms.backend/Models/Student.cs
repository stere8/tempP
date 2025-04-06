using Microsoft.AspNetCore.Identity;

namespace sms.backend.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Link to IdentityUser
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
