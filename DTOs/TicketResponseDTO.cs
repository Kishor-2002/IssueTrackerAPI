namespace IssueTrackerAPI.DTOs
{
    public class TicketResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        public string ProjectName { get; set; }
        public string AssignedToEmail { get; set; }
    }
}
