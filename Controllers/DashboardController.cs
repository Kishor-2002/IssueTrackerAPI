using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        //[Authorize] // 👈 Apply here
        //[HttpGet("/")]
        //public async Task<IActionResult> AllTickets(TicketCreateDTO ticketDto)
        //{
        //    var tickets = await _context.Tickets
        //        //.Include(t => t.Project)
        //        //.Include(t => t.AssignedTo)
        //        .ToListAsync();
        //    return Ok(tickets);
        //}
        //[Authorize] // 👈 Apply here
        //[HttpGet("/open")]
        //public async Task<IActionResult> OpenTickets()
        //{
        //    var openTickets = await _context.Tickets.Where(x => x.Status.Equals("Todo")).ToListAsync();
        //    return Ok(openTickets);
        //}
        //[Authorize] // 👈 Apply here
        //[HttpGet("/close")]
        //public async Task<IActionResult> ClosedTickets()
        //{
        //    var openTickets = await _context.Tickets.Where(x => x.Status.Equals("Completed")).ToListAsync();
        //    return Ok(openTickets);
        //}
        //[Authorize] // 👈 Apply here
        //[HttpGet("/projects")]
        //public async Task<IActionResult> Projects()
        //{
        //    var projects = await _context.Projects.ToListAsync();
        //    return Ok(projects);
        //}
        [Authorize] // 👈 Apply here
        [HttpGet("/assignedToMe")]
        public async Task<IActionResult> AssignedTickets()
        {
            //Check with user Id, 2 users can have same name    
            var assignedTickets = await _context.Tickets.Where(x => x.AssignedTo.Name == HttpContext.User.Identity.Name).ToListAsync();
            return Ok(assignedTickets);
        }


        [Authorize] // 👈 Apply here
        [HttpGet("/")]
        public async Task<IActionResult> TotalTickets(TicketCreateDTO ticketDto)
        {
            var tickets = await _context.Tickets
                //.Include(t => t.Project)
                //.Include(t => t.AssignedTo)
                .ToListAsync();
            return Ok(tickets.Count());
        }
        [Authorize] // 👈 Apply here
        [HttpGet("/open")]
        public async Task<IActionResult> TotalOpenTickets()
        {
            var openTickets = await _context.Tickets.Where(x => x.Status.Equals("Todo")).ToListAsync();
            return Ok(openTickets.Count());
        }
        [Authorize] // 👈 Apply here
        [HttpGet("/close")]
        public async Task<IActionResult> TotalClosedTickets()
        {
            var openTickets = await _context.Tickets.Where(x => x.Status.Equals("Completed")).ToListAsync();
            return Ok(openTickets.Count());
        }
        [Authorize] // 👈 Apply here
        [HttpGet("/projects")]
        public async Task<IActionResult> TotalProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects.Count());
        }
    }
}

//show total tickets

//total open

//total closed

//total projects

//your assigned tickets