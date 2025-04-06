using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TeacherEnrollment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TeacherEnrollmentId { get; set; }
    public int StaffId { get; set; }
    public int ClassId { get; set; }
    public int LessonId { get; set; }

}