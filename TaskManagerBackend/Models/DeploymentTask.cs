namespace TaskManagerBackend.Models
{
    public class DeploymentTask : TaskBase
    {
        public DateTime Deadline { get; set; }
        public string DeploymentScope { get; set; } // max 400 chars
    }
}
