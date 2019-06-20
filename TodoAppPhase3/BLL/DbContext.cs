using System.Data.Entity;

namespace TodoAppPhase3.BLL
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("name = TodoAppPhase3ConnectionString")
        {

        }
        public DbSet<Task> Task { get; set; }

    }
}
