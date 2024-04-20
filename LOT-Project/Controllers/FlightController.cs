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

        public ActionResult<FlightDto> GetAll()
        {
            try
            {
                var flights = _flightService.GetAll();

                return Ok(flights);
            }
            catch (NotFoundExeption ex)
            {
                return NotFound(ex.Message);
            }

            //var flights = _dbContext.Flights.ToList();
            //if (flights == null)
            //{
            //    return NotFound();
            //}
            //return Ok(flights);
        }
        [HttpPost("/add")]
        public ActionResult<FlightDto> AddFlight([FromBody] FlightDto dto)
        {
            try
            {
                _flightService.Add(dto);
                return Ok(dto);
            }
            catch (BadRequestExeption ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
