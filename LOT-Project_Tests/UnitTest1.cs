using AutoMapper;
using LOT_Project.Controllers;
using LOT_Project.Entities;
using LOT_Project.Models;
using LOT_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net.Http.Headers;
using AutoMapper;
using LOT_Project;
using Microsoft.EntityFrameworkCore;


namespace LOT_Project_Tests
{

    public class UnitTest1
    {
        private readonly IMapper _mapper;
        private readonly FlightsDbContext _dbContext;


        private static string globalToken;
        public UnitTest1(FlightsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Fact]
        public void Login()
        {
            var mockAccountService = new Mock<IAccountService>();

            string expectedToken = "fake_jwt_token";
            mockAccountService.Setup(x => x.GenerateJwt(It.IsAny<LoginDto>())).Returns(expectedToken);

            var accountController = new AccountController(mockAccountService.Object, null);

            var loginDto = new LoginDto
            {
                Login = "mrychert",
                Password = "haslo123"
            };

            var result = accountController.Login(loginDto);

            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            globalToken = okResult.Value.ToString();

            Assert.Equal(expectedToken, globalToken);
        }

        [Fact]
        public void GetAllFlights_ReturnsAllFlights()
        {

            var flights = new List<Flight>
        {
            new Flight { id = 1, flightNumber = "LO123", departureDate = DateTime.Now, departurePoint = "Warszawa", arrivalPoint = "Kraków", aircraftType = "Boeing 737" },
            new Flight { id = 2, flightNumber = "LO456", departureDate = DateTime.Now, departurePoint = "Kraków", arrivalPoint = "Warszawa", aircraftType = "Airbus A320" },
        };
            var flightDtos = _mapper.Map<List<FlightDto>>(flights);

            var mockFlightService = new Mock<IFlightService>();
            mockFlightService.Setup(x => x.GetAll()).Returns(flightDtos);
            var flightService = new FlightService(_dbContext, _mapper);

            var flightController = new FlightController(new FlightsDbContext(), _mapper, flightService);
            var result = flightController.GetAll();

            Assert.IsType<OkObjectResult>(result);

            var okResult = result.Result as OkObjectResult;
            var flightsResult = okResult.Value as List<FlightDto>;

            Assert.Equal(flights.Count, flightsResult.Count);

            foreach (var flight in flights)
            {
                Assert.Contains(flightsResult, f => f.flightNumber == flight.flightNumber);
            }
        }

    }


}