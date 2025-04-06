namespace sms.backend.Models
{
    public class StudentParent
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // Use ParentId as the FK to your Parent entity (or to IdentityUser if you're linking directly)
        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;

        // Mark if this is the primary parent (if needed)
        public bool IsPrimary { get; set; }
    }
}
