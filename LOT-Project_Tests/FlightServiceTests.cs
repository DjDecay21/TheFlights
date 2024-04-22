using AutoMapper;
using LOT_Project;
using LOT_Project.Entities;
using LOT_Project.Exeptions;
using LOT_Project.Models;
using LOT_Project.Repositories;
using LOT_Project.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System;

namespace LOT_Project_Tests
{
    public class FlightServiceTests
    {
        [Fact]
        public void GetAll_WhenSuccess_ReturnFlights()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.Get())
                .Returns(new List<Flight> { new Flight { flightNumber="QW987" } });

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            var result = sut.GetAll();

            //Assert
            Assert.Equal("QW987", result[0].flightNumber);
        }

        [Fact]
        public void GetAll_WhenMultipleFlight_ReturnFlightsList()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.Get())
                .Returns(new List<Flight> { new(), new() });

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            var result = sut.GetAll();

            //Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAll_NoFlights_NotFoundExceptionThrown()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.Get())
                .Returns(new List<Flight>());

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            var action = sut.GetAll;

            //Assert
            Assert.Throws<NotFoundExeption>(action);
        }

        [Fact]
        public void Add_FlightExists_BadRequestExeptionThrown()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.GetByFlightNumber(It.IsAny<string>()))
                .Returns(new Flight());

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            var action = () => sut.Add(new FlightDto());

            //Assert
            Assert.Throws<BadRequestExeption>(action);
        }

        [Fact]
        public void Add_ValidFlight_FlightAdded()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.GetByFlightNumber(It.IsAny<string>()))
                .Returns((Flight?)null);

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            sut.Add(new FlightDto
            {
                aircraftType = "Airbus",
                arrivalPoint = "abc",
                departureDate = DateTime.Now,
                departurePoint = "abc",
                flightNumber = "RE432"
            });

            //Assert
            flightsRepositoryMock.Verify(x => x.Add(It.IsAny<Flight>()), Times.Once);
        }

        [Fact]
        public void Add_ValidFlight_FlightWithValidParametersAdded()
        {
            //Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.GetByFlightNumber(It.IsAny<string>()))
                .Returns((Flight?)null);

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);
            var flight = new FlightDto
            {
                aircraftType = "Airbus",
                arrivalPoint = "abc",
                departureDate = DateTime.Now,
                departurePoint = "abc",
                flightNumber = "RE432"
            };

            //Act
            sut.Add(flight);

            //Assert
            flightsRepositoryMock.Verify(x => x.Add(It.Is<Flight>(y => y.aircraftType == flight.aircraftType)), Times.Once);
            flightsRepositoryMock.Verify(x => x.Add(It.Is<Flight>(y => y.flightNumber == flight.flightNumber)), Times.Once);
            flightsRepositoryMock.Verify(x => x.Add(It.Is<Flight>(y => y.departureDate == flight.departureDate)), Times.Once);
            flightsRepositoryMock.Verify(x => x.Add(It.Is<Flight>(y => y.arrivalPoint == flight.arrivalPoint)), Times.Once);
            flightsRepositoryMock.Verify(x => x.Add(It.Is<Flight>(y => y.departurePoint == flight.departurePoint)), Times.Once);
        }
        [Fact]
        public void Update_FlightNoExists_BadRequestExeption()
        {
            // Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightInstance = new Flight(); 
            var updateFlightDtoInstance = new UpdateFlightDto(); 

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            // Act
            flightsRepositoryMock.Setup(x => x.Update(flightInstance, updateFlightDtoInstance));
            var action = () => sut.Add(new FlightDto());

            // Assert
            Assert.Throws<BadRequestExeption>(action);
        }
        [Fact]
        public void Update_WhenSuccess_ReturnUpdatedFlight()
        {
            //Arange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightInstance = new Flight{
                id=5,
                flightNumber="PL111",
                departureDate= DateTime.Now,
                departurePoint="Krakow",
                arrivalPoint="Poznan",
                aircraftType="Boeing"
            };
            var updateFlightDtoInstance = new UpdateFlightDto
            {
                
                flightNumber="LP222",
                departureDate= DateTime.Now,
                departurePoint="Gdansk",
                arrivalPoint="Bydgoszcz",
                aircraftType= "Embraer"
            };

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            flightsRepositoryMock.Setup(x => x.GetById(5)).Returns(flightInstance);
            flightsRepositoryMock.Setup(x => x.Update(flightInstance, updateFlightDtoInstance))
                                 .Callback(() =>
                                 {
                                     flightInstance.flightNumber = updateFlightDtoInstance.flightNumber;
                                     flightInstance.departureDate = updateFlightDtoInstance.departureDate;
                                     flightInstance.departurePoint = updateFlightDtoInstance.departurePoint;
                                     flightInstance.arrivalPoint = updateFlightDtoInstance.arrivalPoint;
                                     flightInstance.aircraftType = updateFlightDtoInstance.aircraftType;
                                 });

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            //Act
            sut.Update(5, updateFlightDtoInstance);

            //Assert
            flightsRepositoryMock.Verify(x => x.Update(
                It.Is<Flight>(y =>
                    y.aircraftType == "Boeing" || 
                    y.aircraftType == "Embraer" ||
                    y.aircraftType == "Airbus" &&
                    y.flightNumber == updateFlightDtoInstance.flightNumber &&
                    y.departureDate == updateFlightDtoInstance.departureDate &&
                    y.arrivalPoint == updateFlightDtoInstance.arrivalPoint &&
                    y.departurePoint == updateFlightDtoInstance.departurePoint),
                It.IsAny<UpdateFlightDto>()),
                Times.Once);

        }
        [Fact]
        public void Delete_FlightNoExsist_NotFoundExeption()
        {
            // Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightInstance = new Flight();
            var flightId = 4;
            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);

            // Act
            flightsRepositoryMock.Setup(x => x.GetById(5))
                 .Returns(flightInstance);
            flightsRepositoryMock.Setup(x => x.Delete(flightInstance));
            var action = () => sut.Delete(flightId);

            // Assert
            Assert.Throws<NotFoundExeption>(action);
        }
        [Fact]
        public void Delete_FlightExists_FlightWithValidParametersDeleted()
        {
            // Arrange
            var dbContext = new FlightsDbContext();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var flightsRepositoryMock = new Mock<IFlightsRepository>();
            var flightInstance = new Flight
            {
                id = 5,
                flightNumber = "PL111",
                departureDate = DateTime.Now,
                departurePoint = "Krakow",
                arrivalPoint = "Poznan",
                aircraftType = "Boeing"
            };
            flightsRepositoryMock.Setup(x => x.GetById(5))
                .Returns(flightInstance); // Zwraca istniejący lot

            var sut = new FlightService(dbContext, mapper, flightsRepositoryMock.Object);
            var flightIdToDelete = 5;

            // Act
            sut.Delete(flightIdToDelete);

            // Assert
            // Sprawdzamy, czy metoda Delete została wywołana tylko jeśli lot istnieje
            flightsRepositoryMock.Verify(x => x.Delete(It.IsAny<Flight>()), Times.Once);
        }


    }
}
