using IssueTrackerAPI.DTOs;
using IssueTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
    //[HttpPost("create")]
    [HttpPost]
    public async Task<IActionResult> Create(ProjectDTO projectDto)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(projectDto.Name))
            {
                throw new Exception("Project name cannot be empty.");
            }
            if(_context.Projects.Any(p => p.Name == projectDto.Name))
            {
                throw new Exception("Project with this name already exists.");
            }
            var project = new Project
            {
                Name = projectDto.Name
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return Ok(project);
        }catch (Exception ex)
        {
            return BadRequest($"Given project name already exists : {ex.Message}");
        }

    }
    [Authorize] // 👈 Apply here
    [HttpPatch("{projectId}/tickets")]
    public async Task<IActionResult> GetTicketsByProject([FromRoute] int projectId )
    {
        //var projectId = HttpContext.Request.ro["projectId"];
        var tickets = _context.Tickets
                    .Include(t => t.Project)
                    .Include(t => t.AssignedTo)
                    .Where(x => x.ProjectId == projectId).ToList();
        return Ok(tickets);
    }
    [Authorize] // Any logged-in user can access this
    [HttpGet]
    public IActionResult GetProjects()
    {
        return Ok(_context.Projects.ToList());
    }
}
