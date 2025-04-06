using Microsoft.AspNetCore.Identity;

namespace sms.backend.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SubjectExpertise { get; set; } = string.Empty;

        // Link to IdentityUser
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
