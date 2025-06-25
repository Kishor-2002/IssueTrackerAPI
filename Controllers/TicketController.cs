using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IssueTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")] // 👈 Apply here
        [HttpPost("create")]
        public async Task<IActionResult> Create(TicketCreateDTO ticketDto)
        {
            //More work pending
            Ticket ticket = new Ticket()
            {
                Title = ticketDto.Title,
                //Description = ticketDto.Description,
                //Status = ticketDto.Status,
                //AssignedTo = ticketDto.AssignedTo,
                AssignedToId= ticketDto.AssignedToId??0,
                ProjectId = ticketDto.ProjectId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        [Authorize(Roles = "admin,developer")] // 👈 Apply here
        [HttpPatch("assign/{id}")]
        public async Task<IActionResult> UpdateTickets(TicketCreateDTO ticketDto)
        {
            return null; // Placeholder for the actual implementation
        }
        [HttpPatch("assign/{id}")]
        public async Task<IActionResult> AssignedToMe()
        {
            return _context.Tickets.Where(x => x.AssignedTo == HttpContext.User.Identity.Name).ToList() switch
            {
                var tickets when tickets.Any() => Ok(tickets),
                _ => NotFound("No tickets assigned to you.")
            };
        }
    }
}
