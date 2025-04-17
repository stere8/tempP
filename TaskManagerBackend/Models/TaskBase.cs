
using static TaskManagerBackend.Enums;

namespace TaskManagerBackend.Models
{
    public abstract class TaskBase
    {
        public int Id { get; init; }
        public string ShortDescription { get; init; }
        public int Difficulty { get; init; }
        public TaskType TaskType { get; init; }
        public int? AssignedUserId { get; set; }
    }
}
