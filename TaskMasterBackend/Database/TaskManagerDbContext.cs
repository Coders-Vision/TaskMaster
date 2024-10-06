using Microsoft.EntityFrameworkCore;

namespace TaskMasterBackend.Database
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Task> Tasks { get; set;}
    }
}
