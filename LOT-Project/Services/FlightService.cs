using AutoMapper;
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
        List<FlightDto> GetAll();
    }
    public class FlightService : IFlightService
    {
        private readonly FlightsDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public FlightService(FlightsDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public List<FlightDto> GetAll()
        {
            var flights = _dbContext.Flights.ToList();
            if (flights == null)
            {
                throw new NotFoundExeption("No data");
            }
            return flights.Select(flight => new FlightDto
            {
                flightNumber = flight.flightNumber,
                departureDate = flight.departureDate,
                departurePoint = flight.departurePoint,
                arrivalPoint = flight.arrivalPoint,
                aircraftType = flight.aircraftType
            }).ToList();
        }
        public void Add(FlightDto dto)
        {
            var checkFlight = _dbContext
                .Flights
                .FirstOrDefault(u => u.flightNumber == dto.flightNumber);
            if (checkFlight != null)
            {
                throw new BadRequestExeption("The flight already exists");

            }
            if (string.IsNullOrWhiteSpace(dto.flightNumber) ||
                string.IsNullOrWhiteSpace(dto.departurePoint) ||
                string.IsNullOrWhiteSpace(dto.arrivalPoint) ||
                string.IsNullOrWhiteSpace(dto.aircraftType))
            {
                throw new BadRequestExeption("All fields are required.");
            }
            var flight = _mapper.Map<Flight>(dto);
            if (flight == null)
            {
                throw new BadRequestExeption("");
            }
            if (!Regex.IsMatch(flight.flightNumber, @"^[A-Za-z]{2}\d{3}$"))
            {
                throw new BadRequestExeption("Correct Flight Number");
            }
            if (flight.aircraftType != "Embraer" && flight.aircraftType != "Boeing" && flight.aircraftType != "Airbus")
            {
                throw new BadRequestExeption("Correct the aircraftType");

            }
            DateTime currentDateTime = DateTime.Now;

            Flight flight1 = new Flight
            {
                flightNumber = dto.flightNumber,
                departureDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, currentDateTime.Minute, 0),
                departurePoint = dto.departurePoint,
                arrivalPoint = dto.arrivalPoint,
                aircraftType = dto.aircraftType,
            };
            _dbContext.Flights.Add(flight1);
            _dbContext.SaveChanges();
            //throw new CreateExeption($"{flight1}");

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
                throw new BadRequestExeption("No change");
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
