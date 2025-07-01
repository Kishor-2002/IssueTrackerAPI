using Microsoft.EntityFrameworkCore;
using IssueTrackerAPI.Models;
    public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<Ticket> Tickets { get; set; }
	public DbSet<TicketComment> TicketComments { get; set; }
}
