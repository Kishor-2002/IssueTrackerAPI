namespace IssueTrackerAPI.DTOs
{
    public class ProjectResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TicketSummaryDTO> Tickets { get; set; }  // nested ticket summary
    }
}
