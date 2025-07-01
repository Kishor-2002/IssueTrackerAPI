using IssueTrackerAPI.Models;

namespace IssueTrackerAPI
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Users.Any())
            {
                var admin = new User
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "admin"
                };
                var dev = new User
                {
                    Name = "Developer",
                    Email = "developer@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Dev@123"),
                    Role = "developer"
                };
                var tester = new User
                {
                    Name = "Tester",
                    Email = "tester@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                    Role = "tester"
                };

                _context.Users.AddRange(admin, dev, tester);
            }

            if (!_context.Projects.Any())
            {
                var project1 = new Project { Name = "Website Revamp" };
                var project2 = new Project { Name = "Mobile Onboarding" };
                _context.Projects.AddRange(project1, project2);
            }

            _context.SaveChanges();
        }
    }

}
