namespace IssueTrackerAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}