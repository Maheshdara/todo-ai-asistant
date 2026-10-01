using Microsoft.EntityFrameworkCore;

namespace TdoApi
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Models.TodoTask> TodoTasks { get; set; } = null!;
    }
}
