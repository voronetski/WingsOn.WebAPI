using System;
using System.Linq;
using NUnit.Framework;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.DTO;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Tests.Services
{
    public class BookingServiceTests
    {
        private IBookingService _bookingService;
        private IAsyncRepository<Flight> _flightRepositoryAsync;
        private IAsyncRepository<Booking> _bookingRepositoryAsync;
        private IAsyncRepository<Person> _personRepositoryAsync;

        [SetUp]
        public void Setup()
        {
            _flightRepositoryAsync = new FlightAsyncRepository();
            _bookingRepositoryAsync = new BookingAsyncRepository();
            _personRepositoryAsync = new PersonAsyncRepository();
            _bookingService = new BookingService(_bookingRepositoryAsync, _flightRepositoryAsync, _personRepositoryAsync);
        }

        [Test]
        [TestCase("PZ696", false)]
        [TestCase("", true)]
        [TestCase(null, true)]
        public void GetAllPassengersForFlightValidationTest(string flightNumber, bool expected)
        {
            var result = _bookingService.GetAllPassengersForFlight(flightNumber).Result;
            Assert.That(result == null, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("PZ696")]
        public void GetAllPassengersForFlightTest(string flightNumber)
        {
            var result = _bookingService.GetAllPassengersForFlight(flightNumber).Result;
            var actualResult = _bookingRepositoryAsync.GetAllAsync().Result.Where(booking => string.Equals(booking.Flight.Number, flightNumber, StringComparison.CurrentCultureIgnoreCase)).
                SelectMany(booking => booking.ToBookingDto().Passengers).Count();
            Assert.AreEqual(result.Count(), actualResult);
        }

        [Test]
        // one more test case is needed
        [TestCase("PZ696", null, true)]
        [TestCase("", null, true)]
        [TestCase(null, null,true)]
        public void AddPassengerToExistingFlightValidationTest(string flightNumber, PassengerDTO passenger, bool expected)
        {
            var result = _bookingService.AddPassengerToExistingFlight(flightNumber, passenger).Result;
            Assert.That(result == null, Is.EqualTo(expected));
        }

    }
}