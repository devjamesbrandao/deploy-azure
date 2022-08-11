using MeuTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);
    }
}