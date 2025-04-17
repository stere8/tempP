using TaskManagerBackend;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;
using static TaskManagerBackend.Enums;

var builder = WebApplication.CreateBuilder(args);


// Update the User class initialization to match the correct property name 'UserType' instead of 'Role'.
var users = new List<User>
{
   new User { Id = 1, NameAndSurname = "Alice", UserType = Enums.UserType.Programmer },
   new User { Id = 2, NameAndSurname = "Bob", UserType = Enums.UserType.DevOpsAdministrator },
   new User { Id = 3, NameAndSurname = "Charlie", UserType = Enums.UserType.Programmer },
   new User { Id = 4, NameAndSurname = "Diana", UserType = Enums.UserType.DevOpsAdministrator },
   new User { Id = 5, NameAndSurname = "Ethan", UserType = Enums.UserType.DevOpsAdministrator },
   new User { Id = 6, NameAndSurname = "Fiona", UserType = Enums.UserType.Programmer },
   new User { Id = 7, NameAndSurname = "George", UserType = Enums.UserType.DevOpsAdministrator },
   new User { Id = 8, NameAndSurname = "Hannah", UserType = Enums.UserType.Programmer },
   new User { Id = 9, NameAndSurname = "Ian", UserType = Enums.UserType.DevOpsAdministrator },
   new User { Id = 10, NameAndSurname = "Julia", UserType = Enums.UserType.Programmer }
};

// Fix for CS1026, CS0103: Correcting syntax and ensuring properties exist in the MaintenanceTask initialization.

var tasks = new List<TaskBase>
   {
      // Maintenance Tasks (10 tasks)
      new MaintenanceTask
      {
          Id = 131,
          ShortDescription = "Maintenance for database servers",
          Difficulty = 5,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-01"),
          ListOfServices = "Database backup, patching, and optimization.",
          ListOfServers = "DB-Server-1, DB-Server-2",
          AssignedUserId = 2
      },
      new MaintenanceTask
      {
          Id = 132,
          ShortDescription = "Maintenance for web servers",
          Difficulty = 4,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-05"),
          ListOfServices = "Server updates and load balancing check.",
          ListOfServers = "Web-Server-1, Web-Server-2",
          AssignedUserId = 4
      },
      new MaintenanceTask
      {
          Id = 133,
          ShortDescription = "Maintenance for API servers",
          Difficulty = 3,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-10"),
          ListOfServices = "API health check and performance tuning.",
          ListOfServers = "API-Server-1",
          AssignedUserId = 5
      },
      new MaintenanceTask
      {
          Id = 134,
          ShortDescription = "Maintenance for cache servers",
          Difficulty = 2,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-15"),
          ListOfServices = "Cache clearing and optimization.",
          ListOfServers = "Cache-Server-1",
          AssignedUserId = 7
      },
      new MaintenanceTask
      {
          Id = 135,
          ShortDescription = "Maintenance for security systems",
          Difficulty = 4,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-20"),
          ListOfServices = "Security patch updates and monitoring.",
          ListOfServers = "Security-Server-1, Security-Server-2",
          AssignedUserId = 9
      },
      new MaintenanceTask
      {
          Id = 136,
          ShortDescription = "Maintenance for backup systems",
          Difficulty = 3,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-25"),
          ListOfServices = "Backup verification and system checks.",
          ListOfServers = "Backup-Server-1",
          AssignedUserId = null
      },
      new MaintenanceTask
      {
          Id = 137,
          ShortDescription = "Maintenance - upgrade firmware",
          Difficulty = 2,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-08-30"),
          ListOfServices = "Firmware upgrade for critical servers.",
          ListOfServers = "Server-Upgrade-1",
          AssignedUserId = null
      },
      new MaintenanceTask
      {
          Id = 138,
          ShortDescription = "Maintenance - optimize performance",
          Difficulty = 5,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-09-05"),
          ListOfServices = "System performance optimization and diagnostics.",
          ListOfServers = "Performance-Server-1",
          AssignedUserId = 2
      },
      new MaintenanceTask
      {
          Id = 139,
          ShortDescription = "Maintenance for network devices",
          Difficulty = 4,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-09-10"),
          ListOfServices = "Network configuration and updates.",
          ListOfServers = "Network-Device-1, Network-Device-2",
          AssignedUserId = null
      },
      new MaintenanceTask
      {
          Id = 140,
          ShortDescription = "Maintenance - review system logs",
          Difficulty = 3,
          TaskType = TaskType.Maintenance,
          Deadline = DateTime.Parse("2025-09-15"),
          ListOfServices = "Log analysis and report generation.",
          ListOfServers = "Log-Server-1",
          AssignedUserId = 4
      }
   };

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITaskServices, TaskService>(provider =>
{
    return new TaskService(tasks, users);
});
builder.Services.AddSingleton<List<User>>(users);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
app.MapControllers();
app.Run();
