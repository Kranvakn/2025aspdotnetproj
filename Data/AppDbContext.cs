using Microsoft.EntityFrameworkCore;
using todoApp.Models;

namespace todoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TodoGroup> TodoGroups { get; set; } = null!;
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
    }
}
