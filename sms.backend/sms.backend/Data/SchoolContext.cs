using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sms.backend.Models;

namespace sms.backend.Data
{
    public class SchoolContext : IdentityDbContext<IdentityUser>
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff?> Staff { get; set; }
        public DbSet<Parent?> Parents { get; set; }
        public DbSet<Class?> Classes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<TeacherEnrollment> TeacherEnrollments { get; set; }
        public DbSet<StudentParent> StudentParents { get; set; }  // For parent assignments


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base to configure Identity
            base.OnModelCreating(modelBuilder);

            // Example configuration:
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.EnrollmentId);

            modelBuilder.Entity<TeacherEnrollment>()
                .Property(e => e.TeacherEnrollmentId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<StudentParent>()
                .HasKey(sp => new { sp.StudentId, sp.ParentId });

            // Configure decimal precision for Mark
            modelBuilder.Entity<Mark>()
                .Property(m => m.MarkValue)
                .HasColumnType("decimal(18,2)");


            // Student and IdentityUser relationship
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany() // or .WithOne() if a user can be linked to only one student
                .HasForeignKey(s => s.UserId);

            // Staff and IdentityUser relationship
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            // Configure the composite key for StudentParent join table
            modelBuilder.Entity<StudentParent>()
                .HasKey(sp => new { sp.StudentId, sp.ParentId });

            // Optional: Define relationship between Parent and IdentityUser if needed
            modelBuilder.Entity<Parent>()
                .HasOne(p => p.User)
                .WithMany() // or customize as needed
                .HasForeignKey(p => p.UserId);

            // Define other relationships and keys as needed
            modelBuilder.Entity<Student>().HasData(
        new Student { StudentId = 1, FirstName = "Léon", LastName = "Mugenzi", DateOfBirth = DateTime.Parse("2015-01-10") },
        new Student { StudentId = 2, FirstName = "Marie", LastName = "Mukamana", DateOfBirth = DateTime.Parse("2015-02-15") },
        new Student { StudentId = 3, FirstName = "Henri", LastName = "Ndayisenga", DateOfBirth = DateTime.Parse("2015-05-30") },
        new Student { StudentId = 4, FirstName = "Claire", LastName = "Mukarutabana", DateOfBirth = DateTime.Parse("2015-06-05") },
        new Student { StudentId = 5, FirstName = "Pierre", LastName = "Kamanzi", DateOfBirth = DateTime.Parse("2015-09-20") },
        new Student { StudentId = 6, FirstName = "Isabelle", LastName = "Ingabire", DateOfBirth = DateTime.Parse("2015-10-25") },
        new Student { StudentId = 7, FirstName = "Michel", LastName = "Munyaneza", DateOfBirth = DateTime.Parse("2015-01-10") },
        new Student { StudentId = 8, FirstName = "Alice", LastName = "Murekatete", DateOfBirth = DateTime.Parse("2015-02-15") },
        new Student { StudentId = 9, FirstName = "Georges", LastName = "Uwimana", DateOfBirth = DateTime.Parse("2015-05-30") },
        new Student { StudentId = 10, FirstName = "Catherine", LastName = "Mukasine", DateOfBirth = DateTime.Parse("2015-06-05") },
        new Student { StudentId = 11, FirstName = "Denis", LastName = "Habimana", DateOfBirth = DateTime.Parse("2015-09-20") },
        new Student { StudentId = 12, FirstName = "Brigitte", LastName = "Mukamugisha", DateOfBirth = DateTime.Parse("2015-10-25") },
        new Student { StudentId = 13, FirstName = "André", LastName = "Uwimbabazi", DateOfBirth = DateTime.Parse("2015-01-10") },
        new Student { StudentId = 14, FirstName = "Christine", LastName = "Murema", DateOfBirth = DateTime.Parse("2015-02-15") },
        new Student { StudentId = 15, FirstName = "Bernard", LastName = "Kamanzi", DateOfBirth = DateTime.Parse("2015-05-30") },
        new Student { StudentId = 16, FirstName = "Jeanne", LastName = "Mukashyaka", DateOfBirth = DateTime.Parse("2015-06-05") },
        new Student { StudentId = 17, FirstName = "Louis", LastName = "Mukankuranga", DateOfBirth = DateTime.Parse("2014-04-10") },
        new Student { StudentId = 18, FirstName = "Lola", LastName = "Rukundo", DateOfBirth = DateTime.Parse("2014-09-12") },
        new Student { StudentId = 19, FirstName = "Zoe", LastName = "Murema", DateOfBirth = DateTime.Parse("2014-05-22") },
        new Student { StudentId = 20, FirstName = "Anna", LastName = "Karekezi", DateOfBirth = DateTime.Parse("2014-01-27") }
   );

            modelBuilder.Entity<Class>().HasData(
             new Class { ClassId = 1, Name = "P3A", GradeLevel = 3, Year = 2024 },
             new Class { ClassId = 2, Name = "P3B", GradeLevel = 3, Year = 2024 },
             new Class { ClassId = 3, Name = "P4A", GradeLevel = 4, Year = 2024 },
             new Class { ClassId = 4, Name = "P4B", GradeLevel = 4, Year = 2024 }
         );

            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@mail.com", SubjectExpertise = "Mathematics" },
                new Staff { StaffId = 2, FirstName = "Bob", LastName = "Williams", Email = "bob.williams@mail.com", SubjectExpertise = "Science" },
                new Staff { StaffId = 3, FirstName = "Charlie", LastName = "Linguist", Email = "charlie.linguist@mail.com", SubjectExpertise = "Kinyarwanda" },
                new Staff { StaffId = 4, FirstName = "Diana", LastName = "English", Email = "diana.english@mail.com", SubjectExpertise = "English" },
                new Staff { StaffId = 5, FirstName = "Edward", LastName = "Historian", Email = "edward.historian@mail.com", SubjectExpertise = "Social Studies" },
                new Staff { StaffId = 6, FirstName = "Fiona", LastName = "Coach", Email = "fiona.coach@mail.com", SubjectExpertise = "Physical Education" },
                new Staff { StaffId = 7, FirstName = "Grace", LastName = "Matheson", Email = "grace.matheson@mail.com", SubjectExpertise = "Mathematics" },
                new Staff { StaffId = 8, FirstName = "Henry", LastName = "Scientist", Email = "henry.scientist@mail.com", SubjectExpertise = "Science" },
                new Staff { StaffId = 9, FirstName = "Isabel", LastName = "Linguist", Email = "isabel.linguist@mail.com", SubjectExpertise = "Kinyarwanda" },
                new Staff { StaffId = 10, FirstName = "Jack", LastName = "English", Email = "jack.english@mail.com", SubjectExpertise = "English" },
                new Staff { StaffId = 11, FirstName = "Karen", LastName = "Historian", Email = "karen.historian@mail.com", SubjectExpertise = "Social Studies" },
                new Staff { StaffId = 12, FirstName = "Leo", LastName = "Coach", Email = "leo.coach@mail.com", SubjectExpertise = "Physical Education" }
            );

        }
    }
}