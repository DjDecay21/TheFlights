using AutoMapper;
using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Repositories;
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
        private readonly IFlightsRepository _flightsRepository;
        private readonly IMapper _mapper;
        
        public FlightService(FlightsDbContext dbContext, IMapper mapper, IFlightsRepository flightsRepository)
        {
            _flightsRepository = flightsRepository;
            _mapper = mapper;
        }
        public List<FlightDto> GetAll()
        {
            var flights = _flightsRepository.Get();
            if (!flights.Any())
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
            var checkFlight = _flightsRepository.GetByFlightNumber(dto.flightNumber);
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

            if (!Regex.IsMatch(flight.flightNumber, @"^[A-Za-z]{2}\d{3}$"))
            {
                throw new BadRequestExeption("Incorrect Flight Number");
            }
            if (flight.aircraftType != "Embraer" && flight.aircraftType != "Boeing" && flight.aircraftType != "Airbus")
            {
                throw new BadRequestExeption("Incorrect the aircraftType");

            }
            _flightsRepository.Add(flight);

        }
        public void Delete(int id)
        {
            var flight = _flightsRepository.GetById(id);
            if(flight is null)
            {
                throw new NotFoundExeption("Flight not found");
                
            }
            _flightsRepository.Delete(flight);
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
            
            var flight = _flightsRepository.GetById(id);

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
            _flightsRepository.Update(flight, dto);
        }
    }
}
