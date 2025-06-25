using IssueTrackerAPI.Models;

namespace IssueTrackerAPI.DTOs
{
    public class ProjectDTO
    {
        public int Id;
        public string Name;
        public List<Ticket> Tickets;
    }
}
