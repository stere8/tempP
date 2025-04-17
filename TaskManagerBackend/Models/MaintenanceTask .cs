namespace TaskManagerBackend.Models
{
    public class MaintenanceTask : TaskBase
    {
        public DateTime Deadline { get; set; }
        public string ListOfServices { get; set; } // max 400 chars
        public string ListOfServers { get; set; }  // max 400 chars
    }
}
