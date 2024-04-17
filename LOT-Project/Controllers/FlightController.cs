using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;

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
            var flight = _mapper.Map<Flight>(dto);
            if(flight == null)
            {
                return BadRequest();
            }
            if(flight.aircraftType == "Embraer"|| flight.aircraftType == "Boeing"|| flight.aircraftType == "Airbus")
            {
                Flight flight1 = new Flight
                {
                    flightNumber = dto.flightNumber,
                    departureDate = dto.departureDate,
                    departurePoint = dto.departurePoint,
                    arrivalPoint = dto.arrivalPoint,
                    aircraftType = dto.aircraftType,
                };
                _dbContext.Flights.Add(flight1);
                _dbContext.SaveChanges();
                return Created($"/api/flight", flight1 );
            }
            return BadRequest("Correct the aircraftType");
            
        }
        [HttpDelete("/delete/{id}/")]
        public ActionResult<FlightDto> Delete([FromRoute] int id)
        {
            _flightService.Delete(id);

            return NoContent();
        }
    }
}
