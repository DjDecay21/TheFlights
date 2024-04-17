using Microsoft.EntityFrameworkCore;

namespace LOT_Project.Entities
{
    public class FlightsDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=Flight;Trusted_Connection=True";

        public DbSet<Flight> Flights { get; set; }
        public DbSet<User>Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Login)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired();

            modelBuilder.Entity<Flight>()
                .Property(x => x.aircraftType)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x => x.departurePoint)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x => x.arrivalPoint)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x => x.departureDate)
                .IsRequired();
            modelBuilder.Entity<Flight>()
                .Property(x => x.flightNumber)
                .IsRequired();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
