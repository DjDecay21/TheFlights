using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace LOT_Project.Services
{
    public interface IFlightService
    {
        void Delete(int id);
    }
    public class FlightService : IFlightService
    {
        private readonly FlightsDbContext _dbContext;
        
        public FlightService(FlightsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Delete(int id)
        {
            var flight = _dbContext
                .Flights
                .FirstOrDefault(r=>r.id == id);
            if(flight is null)
            {
                throw new NotFoundExeption("Flight not found");
                
            }
            _dbContext.Flights.Remove(flight);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateFlightDto dto)
        {
            var flight = _dbContext
                .Flights
                .FirstOrDefault(r=>r.id==id);
            if (flight is null)
                throw new NotFoundExeption("Flight not found");

            flight.flightNumber = dto.flightNumber;
            flight.departureDate = dto.departureDate;
            flight.departurePoint = dto.departurePoint;
            flight.arrivalPoint = dto.arrivalPoint;
            flight.aircraftType = dto.aircraftType;
            _dbContext.SaveChanges();
        }
    }
}
