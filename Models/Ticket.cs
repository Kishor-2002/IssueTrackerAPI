namespace IssueTrackerAPI.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // ToDo, InProgress, Done
        public User AssignedTo { get; set; }
        public int AssignedToId { get; set; }
        public Project Project { get; set; } 
        public int ProjectId { get; set; } // Assuming a Ticket belongs to a Project
    }
}