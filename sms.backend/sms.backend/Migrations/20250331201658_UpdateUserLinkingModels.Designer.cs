﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using sms.backend.Data;

#nullable disable

namespace sms.backend.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20250331201658_UpdateUserLinkingModels")]
    partial class UpdateUserLinkingModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<int>("GradeLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("ClassId");

                    b.ToTable("Classes");

                    b.HasData(
                        new
                        {
                            ClassId = 1,
                            GradeLevel = 3,
                            Name = "P3A",
                            Year = 2024
                        },
                        new
                        {
                            ClassId = 2,
                            GradeLevel = 3,
                            Name = "P3B",
                            Year = 2024
                        },
                        new
                        {
                            ClassId = 3,
                            GradeLevel = 4,
                            Name = "P4A",
                            Year = 2024
                        },
                        new
                        {
                            ClassId = 4,
                            GradeLevel = 4,
                            Name = "P4B",
                            Year = 2024
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TeacherEnrollment", b =>
                {
                    b.Property<int>("TeacherEnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeacherEnrollmentId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<int>("StaffId")
                        .HasColumnType("int");

                    b.HasKey("TeacherEnrollmentId");

                    b.ToTable("TeacherEnrollments");
                });

            modelBuilder.Entity("sms.backend.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("AttendanceId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("sms.backend.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("sms.backend.Models.Lesson", b =>
                {
                    b.Property<int>("LessonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LessonId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GradeLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LessonId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("sms.backend.Models.Mark", b =>
                {
                    b.Property<int>("MarkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MarkId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<decimal>("MarkValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("MarkId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("sms.backend.Models.Parent", b =>
                {
                    b.Property<int>("ParentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParentId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("sms.backend.Models.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResourceId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UploadedBy")
                        .HasColumnType("int");

                    b.HasKey("ResourceId");

                    b.HasIndex("ClassId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("sms.backend.Models.Staff", b =>
                {
                    b.Property<int>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StaffId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectExpertise")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StaffId");

                    b.HasIndex("UserId");

                    b.ToTable("Staff");

                    b.HasData(
                        new
                        {
                            StaffId = 1,
                            Email = "alice.johnson@mail.com",
                            FirstName = "Alice",
                            LastName = "Johnson",
                            SubjectExpertise = "Mathematics"
                        },
                        new
                        {
                            StaffId = 2,
                            Email = "bob.williams@mail.com",
                            FirstName = "Bob",
                            LastName = "Williams",
                            SubjectExpertise = "Science"
                        },
                        new
                        {
                            StaffId = 3,
                            Email = "charlie.linguist@mail.com",
                            FirstName = "Charlie",
                            LastName = "Linguist",
                            SubjectExpertise = "Kinyarwanda"
                        },
                        new
                        {
                            StaffId = 4,
                            Email = "diana.english@mail.com",
                            FirstName = "Diana",
                            LastName = "English",
                            SubjectExpertise = "English"
                        },
                        new
                        {
                            StaffId = 5,
                            Email = "edward.historian@mail.com",
                            FirstName = "Edward",
                            LastName = "Historian",
                            SubjectExpertise = "Social Studies"
                        },
                        new
                        {
                            StaffId = 6,
                            Email = "fiona.coach@mail.com",
                            FirstName = "Fiona",
                            LastName = "Coach",
                            SubjectExpertise = "Physical Education"
                        },
                        new
                        {
                            StaffId = 7,
                            Email = "grace.matheson@mail.com",
                            FirstName = "Grace",
                            LastName = "Matheson",
                            SubjectExpertise = "Mathematics"
                        },
                        new
                        {
                            StaffId = 8,
                            Email = "henry.scientist@mail.com",
                            FirstName = "Henry",
                            LastName = "Scientist",
                            SubjectExpertise = "Science"
                        },
                        new
                        {
                            StaffId = 9,
                            Email = "isabel.linguist@mail.com",
                            FirstName = "Isabel",
                            LastName = "Linguist",
                            SubjectExpertise = "Kinyarwanda"
                        },
                        new
                        {
                            StaffId = 10,
                            Email = "jack.english@mail.com",
                            FirstName = "Jack",
                            LastName = "English",
                            SubjectExpertise = "English"
                        },
                        new
                        {
                            StaffId = 11,
                            Email = "karen.historian@mail.com",
                            FirstName = "Karen",
                            LastName = "Historian",
                            SubjectExpertise = "Social Studies"
                        },
                        new
                        {
                            StaffId = 12,
                            Email = "leo.coach@mail.com",
                            FirstName = "Leo",
                            LastName = "Coach",
                            SubjectExpertise = "Physical Education"
                        });
                });

            modelBuilder.Entity("sms.backend.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StudentId");

                    b.HasIndex("UserId");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            StudentId = 1,
                            DateOfBirth = new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Léon",
                            LastName = "Mugenzi"
                        },
                        new
                        {
                            StudentId = 2,
                            DateOfBirth = new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Marie",
                            LastName = "Mukamana"
                        },
                        new
                        {
                            StudentId = 3,
                            DateOfBirth = new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Henri",
                            LastName = "Ndayisenga"
                        },
                        new
                        {
                            StudentId = 4,
                            DateOfBirth = new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Claire",
                            LastName = "Mukarutabana"
                        },
                        new
                        {
                            StudentId = 5,
                            DateOfBirth = new DateTime(2015, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Pierre",
                            LastName = "Kamanzi"
                        },
                        new
                        {
                            StudentId = 6,
                            DateOfBirth = new DateTime(2015, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Isabelle",
                            LastName = "Ingabire"
                        },
                        new
                        {
                            StudentId = 7,
                            DateOfBirth = new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Michel",
                            LastName = "Munyaneza"
                        },
                        new
                        {
                            StudentId = 8,
                            DateOfBirth = new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Alice",
                            LastName = "Murekatete"
                        },
                        new
                        {
                            StudentId = 9,
                            DateOfBirth = new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Georges",
                            LastName = "Uwimana"
                        },
                        new
                        {
                            StudentId = 10,
                            DateOfBirth = new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Catherine",
                            LastName = "Mukasine"
                        },
                        new
                        {
                            StudentId = 11,
                            DateOfBirth = new DateTime(2015, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Denis",
                            LastName = "Habimana"
                        },
                        new
                        {
                            StudentId = 12,
                            DateOfBirth = new DateTime(2015, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Brigitte",
                            LastName = "Mukamugisha"
                        },
                        new
                        {
                            StudentId = 13,
                            DateOfBirth = new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "André",
                            LastName = "Uwimbabazi"
                        },
                        new
                        {
                            StudentId = 14,
                            DateOfBirth = new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Christine",
                            LastName = "Murema"
                        },
                        new
                        {
                            StudentId = 15,
                            DateOfBirth = new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Bernard",
                            LastName = "Kamanzi"
                        },
                        new
                        {
                            StudentId = 16,
                            DateOfBirth = new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Jeanne",
                            LastName = "Mukashyaka"
                        },
                        new
                        {
                            StudentId = 17,
                            DateOfBirth = new DateTime(2014, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Louis",
                            LastName = "Mukankuranga"
                        },
                        new
                        {
                            StudentId = 18,
                            DateOfBirth = new DateTime(2014, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Lola",
                            LastName = "Rukundo"
                        },
                        new
                        {
                            StudentId = 19,
                            DateOfBirth = new DateTime(2014, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Zoe",
                            LastName = "Murema"
                        },
                        new
                        {
                            StudentId = 20,
                            DateOfBirth = new DateTime(2014, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Anna",
                            LastName = "Karekezi"
                        });
                });

            modelBuilder.Entity("sms.backend.Models.StudentParent", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("bit");

                    b.HasKey("StudentId", "ParentId");

                    b.HasIndex("ParentId");

                    b.ToTable("StudentParents");
                });

            modelBuilder.Entity("sms.backend.Models.Timetable", b =>
                {
                    b.Property<int>("TimetableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TimetableId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("TimetableId");

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("sms.backend.Models.Parent", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("sms.backend.Models.Resource", b =>
                {
                    b.HasOne("Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("sms.backend.Models.Staff", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("sms.backend.Models.Student", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("sms.backend.Models.StudentParent", b =>
                {
                    b.HasOne("sms.backend.Models.Parent", "Parent")
                        .WithMany("StudentParents")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("sms.backend.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("sms.backend.Models.Parent", b =>
                {
                    b.Navigation("StudentParents");
                });
#pragma warning restore 612, 618
        }
    }
}
