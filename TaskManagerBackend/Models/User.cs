using static TaskManagerBackend.Enums;

namespace TaskManagerBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string NameAndSurname { get; set; } // max 100 chars
        public UserType UserType { get; set; } // max 50 chars
    }
}
