using DA.Entity;
using Microsoft.EntityFrameworkCore;

namespace DA.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Server> Servers { get; set; }
    }
}
