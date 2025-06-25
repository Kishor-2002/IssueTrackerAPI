namespace IssueTrackerAPI.DTOs
{
    public class TicketCreateDTO
    {
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }  // Optional at creation
    }
}
