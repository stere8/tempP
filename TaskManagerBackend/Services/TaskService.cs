using System.Linq;
using TaskManagerBackend.Models;

namespace TaskManagerBackend.Services
{
    public class TaskService : ITaskServices
    {
        private const int MinTaskCount = 5;
        private const int MaxTaskCount = 11;
        private const int LowDifficultyThreshold = 2;
        private const int HighDifficultyThreshold = 4;
        private const double MinHighRatio = 0.10;
        private const double MaxHighRatio = 0.30;
        private const double MaxLowRatio = 0.50;
        private const int MaxLowTasksUnderMin = 2;
        private const int MaxHighTasksUnderMin = 1;
        private const int MaxMediumTasksUnderMin = 2;

        private readonly List<TaskBase> _tasks;
        private readonly List<User> _users;

        private readonly object _assignmentLock = new object();


        public TaskService(List<TaskBase> tasks, List<User> users)
        {
            _tasks = tasks;
            _users = users;
        }

        public Task<List<TaskBase>> GetAllTasks() => Task.FromResult(_tasks);

        public Task<List<TaskBase>> GetTasksByUserIdAsync(int id) =>
            Task.FromResult(_tasks.Where(t => t.AssignedUserId == id).ToList());

        public (bool IsSuccess, string Message) AssignTasks(AssignmentRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null) return (false, "User not found.");

            lock (_assignmentLock)
            {

                var proposedTasks = new List<TaskBase>();
                foreach (var taskId in request.TaskIds)
                {
                    var task = _tasks.FirstOrDefault(t => t.Id == taskId);
                    if (task == null) return (false, $"Task with ID {taskId} not found.");
                    if (task.AssignedUserId != null) return (false, $"Task with ID {taskId} is already assigned.");
                    if (user.UserType == Enums.UserType.Programmer && task.TaskType != Enums.TaskType.Implementation)
                        return (false, $"Task with ID {taskId} is not of type Implementation for programmers.");

                    proposedTasks.Add(task);
                }

                var validation = ValidateAssignmentRequest(user, proposedTasks);
                if (!validation.isValid) return validation;

                proposedTasks.ForEach(t => t.AssignedUserId = user.Id);
            }

            return (true, $"Tasks assigned to user {user.NameAndSurname}.");
        }

        private (bool isValid, string Message) ValidateAssignmentRequest(User user, List<TaskBase> proposedTasks)
        {
            if (user == null) return (false, "User cannot be null.");
            if (proposedTasks?.Any() != true) return (false, "Proposed tasks cannot be null or empty.");

            var currentTasks = GetTasksByUserIdAsync(user.Id).Result;
            var totalTasksCount = currentTasks.Count + proposedTasks.Count;
            var allTasks = currentTasks.Concat(proposedTasks).ToList();

            var low = allTasks.Count(t => t.Difficulty <= LowDifficultyThreshold);
            var high = allTasks.Count(t => t.Difficulty >= HighDifficultyThreshold);
            var medium = totalTasksCount - low - high;

            if (totalTasksCount < MinTaskCount)
            {
                if (low > MaxLowTasksUnderMin)
                    return (false, $"Too many low-difficulty tasks. Max allowed under {MinTaskCount} tasks is {MaxLowTasksUnderMin}. Current: {low}");
                if (high > MaxHighTasksUnderMin)
                    return (false, $"Too many high-difficulty tasks. Max allowed under {MinTaskCount} tasks is {MaxHighTasksUnderMin}. Current: {high}");
                if (medium > MaxMediumTasksUnderMin)
                    return (false, $"Too many medium-difficulty tasks. Max allowed under {MinTaskCount} tasks is {MaxMediumTasksUnderMin}. Current: {medium}");

                return (true, "Valid under low task count (pre-balance enforced).");
            }

            if (totalTasksCount > MaxTaskCount)
                return (false, $"Total task count exceeds {MaxTaskCount}. Current: {totalTasksCount}");

            var highRatio = (double)high / totalTasksCount;
            var lowRatio = (double)low / totalTasksCount;

            if (highRatio < MinHighRatio || highRatio > MaxHighRatio)
                return (false, $"User {user.NameAndSurname} must have {MinHighRatio:P0}–{MaxHighRatio:P0} high difficulty tasks. Current: {highRatio:P0}");

            if (lowRatio > MaxLowRatio)
                return (false, $"User {user.NameAndSurname} cannot have more than {MaxLowRatio:P0} low difficulty tasks. Current: {lowRatio:P0}");

            return (true, "Task assignment is valid.");
        }
    }
}