using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;


[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "admin")] // 👈 Apply here
    [HttpPost("create")]
    public async Task<IActionResult> Create(ProjectDTO projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return Ok(project);
    }
    [Authorize] // 👈 Apply here
    [HttpPatch("{projectId}/tickets")]
    public async Task<IActionResult> GetTicketsByProject()
    {
        var projectId = HttpContext.Request.Query["projectId"];
        var tickets = _context.Tickets.Where(x => x.ProjectId == projectId).ToList();
        return Ok(tickets);
    }
    [Authorize] // Any logged-in user can access this
    [HttpGet]
    public IActionResult GetProjects()
    {
        return Ok(_context.Projects.ToList());
    }
}
