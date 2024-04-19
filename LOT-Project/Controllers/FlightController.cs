using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using System.Text.RegularExpressions;

namespace LOT_Project.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class FlightController : Controller
    {
        private readonly IMapper _mapper;

        private readonly FlightsDbContext _dbContext;
        private readonly FlightService _flightService;
        public FlightController(FlightsDbContext dbContext, IMapper mapper, FlightService flightService)
        {
            _flightService = flightService;
            _mapper = mapper;
            _dbContext = dbContext;
        }



        [HttpGet("/flights")]

        public ActionResult <FlightDto> GetAll()
        {
            var flights = _dbContext.Flights.ToList();
            if(flights == null)
            {
                return NotFound();
            }
            return Ok(flights);
        }
        [HttpPost("/add")]
        public ActionResult<FlightDto> AddFlight([FromBody] FlightDto dto)
        {
            var checkFlight = _dbContext
                .Flights
                .FirstOrDefault(u=>u.flightNumber == dto.flightNumber);
            if (checkFlight is null)
            {
                if (string.IsNullOrWhiteSpace(dto.flightNumber) ||
                string.IsNullOrWhiteSpace(dto.departurePoint) ||
                string.IsNullOrWhiteSpace(dto.arrivalPoint) ||
                string.IsNullOrWhiteSpace(dto.aircraftType))
                {
                    return BadRequest("All fields are required.");
                }
                var flight = _mapper.Map<Flight>(dto);
                if (flight == null)
                {
                    return BadRequest();
                }
                if (!Regex.IsMatch(flight.flightNumber, @"^[A-Za-z]{2}\d{3}$"))
                {
                    return BadRequest("Correct Flight Number");
                }
                if (flight.aircraftType == "Embraer" || flight.aircraftType == "Boeing" || flight.aircraftType == "Airbus")
                {
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
                    return Created($"/api/flight", flight1);
                }
                return BadRequest("Correct the aircraftType");
            }
            return BadRequest("The flight already exists");
            
            
        }
        [HttpDelete("/delete/{id}/")]
        public ActionResult<FlightDto> Delete([FromRoute] int id)
        {
            _flightService.Delete(id);

            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateFlightDto dto, [FromRoute]int id)
        {
            try
            {
                _flightService.Update(id, dto);
                return Ok(dto);
            }
            catch (BadRequestExeption ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
