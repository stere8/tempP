using Microsoft.AspNetCore.Http;

namespace sms.backend.Models
{
    public class ResourceUploadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ClassId { get; set; }
        public int UploadedBy { get; set; }  // Ideally, this should be extracted from user claims.
        public IFormFile File { get; set; }
    }

    public class ResourceUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
