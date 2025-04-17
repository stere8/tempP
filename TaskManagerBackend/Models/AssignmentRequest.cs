namespace TaskManagerBackend.Models
{
    public class AssignmentRequest
    {
        public int UserId { get; set; }
        public List<int> TaskIds { get; set; }
    }
}
