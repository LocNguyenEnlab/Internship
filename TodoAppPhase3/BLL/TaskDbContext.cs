using System.Data.Entity;

namespace TodoAppPhase3.BLL
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext() : base("name = TodoAppPhase3ConnectionString")
        {

        }
        public DbSet<Task> Task { get; set; }

    }
}
