using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.Controllers;
using WingsOn.WebAPI.NetCore.DTO;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private BookingsController _controller;
        private IAsyncRepository<Flight> _flightRepositoryAsync;
        private IAsyncRepository<Booking> _bookingRepositoryAsync;
        private IAsyncRepository<Person> _personRepositoryAsync;

        [SetUp]
        public void Setup()
        {
            var loggerMock = new Mock<ILogger<Booking>>();

            _personRepositoryAsync = new PersonAsyncRepository();
            _bookingRepositoryAsync = new BookingAsyncRepository();
            _flightRepositoryAsync = new FlightAsyncRepository();
            var bookingService = new BookingService(_bookingRepositoryAsync, _flightRepositoryAsync, _personRepositoryAsync);
            _controller = new BookingsController(loggerMock.Object, bookingService);
        }

        [Test]
        [TestCase("", 404)]
        [TestCase(null, 404)]
        public void GetAllPassengersForFlightFailedValidationTest(string flightNumber, int status)
        {
            var result = _controller.GetPassengersByFlight(flightNumber).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }

        [Test]
        [TestCase("PZ696", 200)]
        [TestCase("ZZZ", 200)]
        public void GetAllPassengersForFlightSucceededValidationTest(string flightNumber, int status)
        {
            var result = _controller.GetPassengersByFlight(flightNumber).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            var passengers = result.Value as IEnumerable<PassengerDTO>;
            Assert.IsNotNull(passengers);
            Assert.AreEqual(passengers.Count(), _bookingRepositoryAsync.GetAllAsync().Result.Where(booking=> String.Equals(flightNumber, booking.Flight.Number, StringComparison.CurrentCultureIgnoreCase)).
                SelectMany(booking=>booking.Passengers).Count());
        }

        [Test]
        [TestCase("", null, 404)]
        [TestCase(null, null, 404)]
        public void CreateBookingForFlightFailedValidationTest(string flightNumber, PassengerDTO passenger, int status)
        {
            var result = _controller.GetPassengersByFlight(flightNumber).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }

        [Test]
        [TestCase("PZ696", 200)]
        public void CreateBookingForFlightOkTest(string flightNumber, int status)
        {
            var newPassenger = new PassengerDTO
            {
                Name = "Kendall Velazquez Jr.",
                DateBirth = "09/24/1980",
                Gender = "Male",
                Address = "806-1408 Mi Rd.",
                Email = "aegestas.a.dui@aliquet.ca",
                Id = _bookingRepositoryAsync.GetAllAsync().Result.Max(booking => booking.Id) + 1
            };

            var result = _controller.CreateBooking(flightNumber, newPassenger).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            
            var booking = result.Value as BookingDTO;
            Assert.IsNotNull(booking);
            Assert.AreEqual(booking.Customer.Name, _bookingRepositoryAsync.GetAsync(booking.Id).Result.Customer.Name);
        }

    }
}
