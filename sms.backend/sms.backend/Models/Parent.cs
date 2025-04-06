using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace sms.backend.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Link to IdentityUser
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        // Navigation: a parent can be linked to many students via a join table
        public ICollection<StudentParent> StudentParents { get; set; } = new List<StudentParent>();
    }
}
