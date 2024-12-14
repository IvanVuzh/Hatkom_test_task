using Microsoft.EntityFrameworkCore;

using Api.Models;

namespace Api.Database
{
    public class ProjectContext: DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }

        public DbSet<TimeLog> TimeLogs { get; set; }
    }
}
