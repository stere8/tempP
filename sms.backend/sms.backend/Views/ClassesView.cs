using sms.backend.Models;

namespace sms.backend.Views;

public class ClassesView
{
    public Class? ViewedClass { get; set; }
    public List<Staff> ClassTeachers { get; set; }
    public List<Student> ClassStudents { get; set; }
    public List<TimetableView> ClassTimetable { get; set; }
}