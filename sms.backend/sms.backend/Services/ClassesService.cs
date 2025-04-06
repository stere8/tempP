using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;
using System.Collections.Generic;
using System.Linq;

namespace sms.backend.Services
{
    public class ClassesService
    {
        private readonly SchoolContext _context;

        public ClassesService(SchoolContext context)
        {
            _context = context;
        }
        public ClassesView Fill(int classID)
        {
            List<int> studentsId = _context.Enrollments
                .Where(e => e.ClassId == classID)
                .Select(e => e.StudentId)
                .ToList();

            List<Student> students = _context.Students
                .Where(student => studentsId.Contains(student.StudentId))
                .ToList();

            List<int> teacherIds = _context.TeacherEnrollments
                .Where(e => e.ClassId == classID)
                .Select(e => e.StaffId)
                .ToList();

            List<Staff> classTeachers = _context.Staff
                .Where(teacher => teacherIds.Contains(teacher.StaffId))
                .ToList();
            Class? viewdClass = _context.Classes.FirstOrDefault(classes => classes != null && classes.ClassId == classID);

            List<TimetableView> timetable = GetTimetableForClass(classID);

            return new ClassesView()
            {
                ClassStudents = students,
                ClassTeachers = classTeachers,
                ViewedClass = viewdClass,
                ClassTimetable = timetable
            };
        }

        public  List<Timetable> GetTeacherTimetable(int id)
        {

            var teachersClass =  _context.TeacherEnrollments.Where(te => te.StaffId == id).Select(te => te.LessonId).ToList();

            var timetable =  _context.Timetables.Where(tt => teachersClass.Contains(tt.LessonId)).ToList();
     

            return timetable;
        }

        public List<TimetableView> GetTimetableForClass(int classId)
        {
            var timetables = (from timetable in _context.Timetables
                             join lesson in _context.Lessons
                                 on timetable.LessonId equals lesson.LessonId
                             join teacherEnrollment in _context.TeacherEnrollments
                                 on new { timetable.ClassId, timetable.LessonId } equals new { teacherEnrollment.ClassId, teacherEnrollment.LessonId }
                                 into teGroup
                             from te in teGroup.DefaultIfEmpty()
                             join staff in _context.Staff
                                 on te.StaffId equals staff.StaffId
                                 into staffGroup
                             from s in staffGroup.DefaultIfEmpty()
                             where timetable.ClassId == classId
                             select new TimetableView
                             {
                                 DayOfWeek = timetable.DayOfWeek,
                                 LessonName = lesson.Subject,
                                 StartTime = timetable.StartTime,
                                 EndTime = timetable.EndTime,
                                 TeachersName = s != null ? $"{s.FirstName} {s.LastName}" : "Not Assigned"
                             }).ToList();

            return timetables;
        }
    }
}
