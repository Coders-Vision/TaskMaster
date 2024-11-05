using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMasterBackend.Database.Configuration;

namespace TaskMasterBackend.Database
{
    public class TaskManagerDbContext : IdentityDbContext<AppUser>
    {
        public TaskManagerDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Task> Tasks { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfigruation());
        }
    }
}
