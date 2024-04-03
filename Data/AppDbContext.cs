using Microsoft.EntityFrameworkCore;
using project_mvc.Models;

namespace project_mvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Brand> Brand { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
