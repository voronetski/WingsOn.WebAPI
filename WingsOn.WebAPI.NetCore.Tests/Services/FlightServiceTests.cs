using System;
using System.Linq;
using NUnit.Framework;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Tests.Services
{
    public class FlightServiceTests
    {
        private IFlightService _flightService;
        private IAsyncRepository<Flight> _flightRepositoryAsync;

        [SetUp]
        public void Setup()
        {
            _flightRepositoryAsync = new FlightAsyncRepository();
            _flightService = new FlightService(_flightRepositoryAsync);
        }

        [Test]
        public void GetAllTest()
        {
            var result = _flightService.GetAllAsync().Result;
            Assert.That(result.Count(), Is.EqualTo(_flightRepositoryAsync.GetAllAsync().Result.Count()));
        }

        [Test]
        [TestCase("PZ696", false)]
        [TestCase("", true)]
        [TestCase(null, true)]
        [TestCase("ZZ666", true)]
        public void GetFlightByNumberValidationTest(string flightNumber, bool expected)
        {
            var result = _flightService.GetFlightByNumber(flightNumber).Result;
            Assert.That(result == null, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("PZ696")]
        public void GetFlightByNumberTest(string flightNumber)
        {
            var result = _flightService.GetFlightByNumber(flightNumber).Result;
            var actualResult = _flightRepositoryAsync.GetAllAsync().Result.FirstOrDefault(flight => String.Equals(flight.Number, flightNumber, StringComparison.CurrentCultureIgnoreCase)).Number;
            Assert.AreEqual(result.Number, actualResult);
        }

    }
}