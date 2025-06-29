﻿using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetTicket(int id)
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

    }
}
