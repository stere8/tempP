namespace TaskManagerBackend
{
    public class Enums
    {
        public enum TaskType
        {
            Implementation = 0,
            Deployment = 1,
            Maintenance = 2,
        }

        public enum ImplementationStatus
        {
            ToBeDone,
            Completed,
        }

        public enum UserType
        {
            DevOpsAdministrator,
            Programmer,
        }
    }
}
