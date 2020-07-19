using BarsGroupTest.Data;
using Microsoft.EntityFrameworkCore;

namespace BarsGroupTest.EF
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Info> info { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Info>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.ServerName).HasMaxLength(200);
                builder.Property(x => x.DbName).HasMaxLength(200);
                builder.Property(x => x.CurrentDateTime).HasMaxLength(50);
            });
        }
    }
}
