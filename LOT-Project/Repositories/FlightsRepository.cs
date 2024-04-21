using LOT_Project.Entities;
using LOT_Project.Models;

namespace LOT_Project.Repositories
{
    public class FlightsRepository : IFlightsRepository
    {
        private FlightsDbContext _dbContext { get; set; }

        public FlightsRepository(FlightsDbContext flightsDbContext)
        {
            _dbContext = flightsDbContext;
        }

        public IReadOnlyList<Flight> Get()
        {
            return _dbContext.Flights.ToList();
        }

        public Flight? GetByFlightNumber(string flightNumber)
        {
            return _dbContext.Flights.FirstOrDefault(x => x.flightNumber == flightNumber);
        }

        public void Add(Flight flight)
        {
            _dbContext.Flights.Add(flight);
            _dbContext.SaveChanges();
        }
        public Flight? GetById(int id)
        {
            return _dbContext.Flights.FirstOrDefault(x => x.id == id);
        }
        public void Delete(Flight flight)
        {
            _dbContext.Flights.Remove(flight);
            _dbContext.SaveChanges();
        }
        public void Update(Flight flight, UpdateFlightDto dto)
        {
            flight.flightNumber = dto.flightNumber;
            flight.departureDate = dto.departureDate;
            flight.departurePoint = dto.departurePoint;
            flight.arrivalPoint = dto.arrivalPoint;
            flight.aircraftType = dto.aircraftType;
            _dbContext.SaveChanges();
        }
    }
}
