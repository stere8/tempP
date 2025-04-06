using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sms.backend.Models;

public class Enrollment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }

}