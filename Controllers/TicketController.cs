using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

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
                ProjectId = ticketDto.ProjectId,
                Status = "ToDo", // Default status
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            var response = new TicketResponseDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                ProjectName = ticket?.Project?.Name,
                AssignedToEmail = ticket?.AssignedTo?.Email
            };

            return Ok(response);
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetTicketByProject(int projectId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.ProjectId == projectId);

            if (ticket == null) return NotFound();

            var response = new TicketResponseDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                ProjectName = ticket?.Project?.Name,
                AssignedToEmail = ticket?.AssignedTo?.Email
            };

            return Ok(response);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTicketByUser(int userId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.AssignedToId == userId);

            if (ticket == null) return NotFound();

            var response = new TicketResponseDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                ProjectName = ticket?.Project?.Name,
                AssignedToEmail = ticket?.AssignedTo?.Email
            };

            return Ok(response);
        }
        [Authorize(Roles = "admin,developer")] // 👈 Apply here
        [HttpPatch("assign/{id}")]
        public async Task<IActionResult> UpdateTickets(TicketDTO ticketDto, int id)
        {
            var ticket = await _context.Tickets.FindAsync(ticketDto.Id);
            if (ticket == null)
                return NotFound("Ticket not found.");

            var assignToUser = await _context.Users.FindAsync(id);

            if (assignToUser == null)
                return BadRequest("User not found.");
            ticket.AssignedTo = assignToUser;

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }
        [HttpGet("assign/{id}")]
        public async Task<IActionResult> AssignedToMe()
        {
            return _context.Tickets.Where(x => x.AssignedTo.Name == HttpContext.User.Identity.Name).ToList() switch
            {
                var tickets when tickets.Any() => Ok(tickets),
                _ => NotFound("No tickets assigned to you.")
            };
        }
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin,developer")]
        public async Task<IActionResult> UpdateTicketStatus(int id, string status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound("Ticket not found.");
            // Validate status
            if (!new[] { "ToDo", "InProgress", "Done" }.Contains(status))
                return BadRequest("Invalid status.");
            ticket.Status = status;
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        [HttpPost("{ticketId}/comments")]
        public async Task<IActionResult> AddComments([FromRoute] int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound("Ticket not found.");
            ticket.Comments.Add(new TicketComment
            {
                Content = "Sample comment", // Replace with actual comment content from request body
                CreatedAt = DateTime.UtcNow,
                Author = HttpContext.User.Identity.Name, // Assuming user ID is stored in the identity
                Ticket = ticket,
                TicketId = ticket.Id
            });
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }
        [HttpGet("{ticketId}/comments")]
        public async Task<IActionResult> GetCommentsForTicket([FromRoute] int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null)
                return NotFound("Ticket not found.");
            return Ok(ticket.Comments);
        }
        [HttpGet("?status={status}&assignedTo={userId}&projectId={projectId}")]
        public async Task<IActionResult> GetReqTickets([FromRoute] string status, [FromRoute] int userId, [FromRoute] int projectId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .Where(t =>
                    (string.IsNullOrEmpty(status) || t.Status == status) &&
                    (userId == null || userId == 0 || t.AssignedToId == userId) &&
                    (projectId == null || projectId == 0 || t.ProjectId == projectId))
                .ToListAsync();
            return Ok(tickets);
        }
    }
}
