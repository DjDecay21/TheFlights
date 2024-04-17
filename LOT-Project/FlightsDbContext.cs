using Microsoft.EntityFrameworkCore;

namespace LOT_Project
{
    public class FlightsDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=Flight;Trusted_Connection=True";

        public DbSet<Flight> Flights { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>()
                .Property(x => x.aircraftType)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x=>x.departurePoint)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x => x.arrivalPoint)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x=>x.departureDate)
                .HasConversion(
                v => v.ToString("yyyy-MM-dd HH:mm"),
                v => DateTime.ParseExact(v, "yyyy-MM-dd HH:mm", null))
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x=>x.flightNumber)
                .IsRequired();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
