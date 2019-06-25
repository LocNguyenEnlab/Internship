using System.Data.Entity;

namespace TodoAppPhase3.BLL
{
    public class DataContext : System.Data.Entity.DbContext
    {
        public DataContext() : base("name = TodoAppPhase3ConnectionString")
        {

        }
        public DbSet<Task> Task { get; set; }
        public DbSet<Author> Author { get; set; }
    }
}
