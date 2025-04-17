using TaskManagerBackend.Models;

namespace TaskManagerBackend.Services
{
    public interface ITaskServices
    {
        public (bool IsSuccess, string Message) AssignTasks(AssignmentRequest request);
        Task<List<TaskBase>> GetAllTasks();
        Task<List<TaskBase>> GetTasksByUserIdAsync(int id);
    }
}
