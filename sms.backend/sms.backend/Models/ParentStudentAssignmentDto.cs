namespace sms.backend.Models
{
    public class ParentStudentAssignmentDto
    {
        public int ParentId { get; set; }
        public int StudentId { get; set; }
        public bool IsPrimary { get; set; } // Optional, if needed.
    }

}
