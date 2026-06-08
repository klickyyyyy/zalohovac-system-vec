using Microsoft.EntityFrameworkCore;
using ZalohovacServer.Entities.DB;

namespace ZalohovacServer.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}