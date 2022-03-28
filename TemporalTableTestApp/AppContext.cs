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
    }
}