namespace sms.backend.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; } // e.g., local directory or cloud URL
        public string FileType { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; } // Navigation property for the associated class
        public int UploadedBy { get; set; } // Teacher's ID
        public DateTime UploadDate { get; set; }
    }

}
