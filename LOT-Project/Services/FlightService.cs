using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;

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

            if (string.IsNullOrWhiteSpace(dto.flightNumber) ||
                string.IsNullOrWhiteSpace(dto.departurePoint) ||
                string.IsNullOrWhiteSpace(dto.arrivalPoint) ||
                string.IsNullOrWhiteSpace(dto.aircraftType))
            {
                throw new BadRequestExeption("Fill in all values");
            }
            
            var flight = _dbContext
                        .Flights
                        .FirstOrDefault(r => r.id == id);

            if (flight is null)
                throw new NotFoundExeption("Flight not found");

            if (!Regex.IsMatch(dto.flightNumber, @"^[A-Za-z]{2}\d{3}$"))
            {
                throw new BadRequestExeption("Correct Flight Number");
            }
            if (dto.aircraftType != "Embraer" && dto.aircraftType != "Boeing" && dto.aircraftType != "Airbus")
            {
                throw new BadRequestExeption("Correct aircraft type");
            }

            
            flight.flightNumber = dto.flightNumber;
            flight.departureDate = dto.departureDate;
            flight.departurePoint = dto.departurePoint;
            flight.arrivalPoint = dto.arrivalPoint;
            flight.aircraftType = dto.aircraftType;
            _dbContext.SaveChanges();
        }
    }
}
