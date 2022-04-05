namespace TemporalTableTestApp
{
    using Microsoft.EntityFrameworkCore;

    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TemporalTableTest;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .ToTable("Event", b => b.IsTemporal(
                    b =>
                    {
                        b.HasPeriodStart("SysStart");
                        b.HasPeriodEnd("SysEnd");
                        b.UseHistoryTable("Event_HISTORY");
                    }));
        }
    }
}