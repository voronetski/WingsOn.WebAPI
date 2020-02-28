using System;
using System.Linq;
using NUnit.Framework;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.DTO;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Tests.Services
{
    public class PassengerServiceTests
    {
        private IPassengerService _passengerService;
        private IAsyncRepository<Person> _personRepositoryAsync;

        [SetUp]
        public void Setup()
        {
            _personRepositoryAsync = new PersonAsyncRepository();
            _passengerService = new PassengerService(_personRepositoryAsync);
        }

        [Test]
        public void GetAllTest()
        {
            var result = _passengerService.GetAll().Result;
            Assert.That(result.Count(), Is.EqualTo(_personRepositoryAsync.GetAllAsync().Result.Count()));
        }

        [Test]
        [TestCase(91, false)]
        [TestCase(0, true)]
        [TestCase(-5, true)]
        public void GetByIdValidationTest(int id, bool expected)
        {
            var result = _passengerService.GetPassengerById(id).Result;
            Assert.That(result == null, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(91)]
        public void GetByIdTest(int id)
        {
            var result = _passengerService.GetPassengerById(id).Result;
            Assert.AreEqual(result.Name, _personRepositoryAsync.GetAsync(id).Result.Name);
        }

        [Test]
        [TestCase("male")]
        [TestCase("female")]
        [TestCase("MALE")]
        [TestCase("FeMALe")]
        public void GetByGenderOkTest(string gender)
        {
            var result = _passengerService.GetByGender(gender).Result;
            Assert.AreEqual(_personRepositoryAsync.GetAllAsync().Result.Count(passenger=>String.Equals(passenger.Gender.ToString(), gender, StringComparison.CurrentCultureIgnoreCase)), result.Count());
        }

        [Test]
        [TestCase("")]
        [TestCase("xxx")]
        public void GetByGenderFailedTest(string gender)
        {
            var result = _passengerService.GetByGender(gender).Result;
            Assert.IsNull(result);
        }

    }
}
