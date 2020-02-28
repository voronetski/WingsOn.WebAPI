using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;
using System.Linq;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.Controllers;
using WingsOn.WebAPI.NetCore.DTO;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Tests.Controllers
{
    public class PassengersControllerTests
    {
        private PassengersController _controller;
        private IAsyncRepository<Person> _personRepository;

        [SetUp]
        public void Setup()
        {
            _personRepository = new PersonAsyncRepository();
            var loggerMock = new Mock<ILogger<PassengerDTO>>();
            var passengerService = new PassengerService(_personRepository);
            _controller = new PassengersController(loggerMock.Object, passengerService);
        }

        [Test]
        public void GetAllTest()
        {
            var result = _controller.GetAll().Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var passengers = result.Value as IEnumerable<PassengerDTO>;
            Assert.IsNotNull(passengers);
            Assert.AreEqual(passengers.Count(), _personRepository.GetAllAsync().Result.Count());
        }

        [Test]
        [TestCase(-5, 404)]
        [TestCase(0, 404)]
        public void GetByIdNotFoundTest(int id, int status)
        {
            var result = _controller.GetPassengerById(id).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }

        [Test]
        [TestCase(91, 200)]
        public void GetByIdOkTest(int id, int status)
        {
            var result = _controller.GetPassengerById(id).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            var passenger = result.Value as PassengerDTO;
            Assert.IsNotNull(passenger);
            var passengerFromRepo = _personRepository.GetAsync(id).Result;
            Assert.AreEqual(passenger.Name, passengerFromRepo.Name);
            Assert.AreEqual(passenger.Address, passengerFromRepo.Address);
        }

        [Test]
        [TestCase(91, "new address", 200)]
        public void UpdateAddressSuccessTest(int id, string newAddress, int status)
        {
            var result = _controller.UpdateAddress(id, newAddress).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            var passenger = result.Value as PassengerDTO;
            Assert.IsNotNull(passenger);
            Assert.AreEqual(newAddress, _personRepository.GetAsync(id).Result.Address);
        }

        [Test]
        [TestCase(0, "new address", 404)]
        [TestCase(-5, "new address", 404)]
        public void UpdateAddressFailedTest(int id, string newAddress, int status)
        {
            var result = _controller.UpdateAddress(id, newAddress).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }

        [Test]
        [TestCase(91, "1@mail.by", 200)]
        public void UpdateEmailAddressSuccessTest(int id, string newEmailAddress, int status)
        {
            var result = _controller.UpdateEmailAddress(id, newEmailAddress).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            var passenger = result.Value as PassengerDTO;
            Assert.IsNotNull(passenger);
            Assert.AreEqual(newEmailAddress, _personRepository.GetAsync(id).Result.Email);
        }

        [Test]
        [TestCase(0, "1@mail.by", 404)]
        [TestCase(-5, "1@mail.by", 404)]
        [TestCase(0, "", 404)]
        [TestCase(-5, "new address", 404)]
        [TestCase(91, "", 404)]
        [TestCase(91, "new address", 404)]
        public void UpdateEmailAddressFailedTest(int id, string newAddress, int status)
        {
            var result = _controller.UpdateEmailAddress(id, newAddress).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }

        [Test]
        [TestCase("male", 200)]
        [TestCase("female", 200)]
        [TestCase("MALE", 200)]
        [TestCase("FeMALe", 200)]
        public void GetByGenderOkTest(string gender, int status)
        {
            var result = _controller.GetByGender(gender).Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
            var passengers = result.Value as IEnumerable<PassengerDTO>;
            Assert.IsNotNull(passengers);
            Assert.AreEqual(_personRepository.GetAllAsync().Result.Count(passenger => String.Equals(passenger.Gender.ToString(), gender, StringComparison.CurrentCultureIgnoreCase)), passengers.Count());
        }

        [Test]
        [TestCase("", 404)]
        [TestCase("xxx", 404)]
        public void GetByGenderFailedTest(string gender, int status)
        {
            var result = _controller.GetByGender(gender).Result as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(status, result.StatusCode);
        }
    }
}
