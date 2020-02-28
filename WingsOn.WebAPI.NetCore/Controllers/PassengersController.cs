using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WingsOn.WebAPI.NetCore.Services;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PassengersController : ControllerBase 
    {
        private readonly ILogger<PassengerDTO> _logger;
        private readonly IPassengerService _passengerService;

        public PassengersController(ILogger<PassengerDTO> logger, IPassengerService passengerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passengerService = passengerService ?? throw new ArgumentNullException(nameof(passengerService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PassengerDTO>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var passengers = await _passengerService.GetAll();
                return Ok(passengers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAll action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }

        }

        // Endpoint that returns a Person by Id.
        [HttpGet("{id}")]
        [HttpGet("get/{id}")]
        [HttpGet("{id}/get")]
        [ProducesResponseType(typeof(PassengerDTO), 200)]
        public async Task<IActionResult> GetPassengerById(int id)
        {
            try
            {
                var person = await _passengerService.GetPassengerById(id);
                if (person == null)
                {
                    _logger.LogError($"Passenger with id: {id} has not been found");
                    return NotFound();
                }

                _logger.LogInformation($"Returned Passenger details with id: {id}");
                return Ok(person);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPersonById action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }

        }

        // Endpoint that lists all the male passengers.
        [HttpGet("gender/{gender}")]
        [HttpGet("get/gender/{gender}")]
        [ProducesResponseType(typeof(IEnumerable<PassengerDTO>), 200)]
        public async Task<IActionResult> GetByGender(string gender)
        {
            try
            {
                var passengers = await _passengerService.GetByGender(gender);
                if (passengers != null) return Ok(passengers);

                _logger.LogError($"Invalid gender {gender} has been passed.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByGender action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }
        }
        
        [HttpPut("get/{id}/address/update")]
        [HttpPut("{id}/address/update")]
        [HttpPatch("get/{id}/address/update")]
        [HttpPatch("{id}/address/update")]
        //Endpoint that updates passenger’s address
        [ProducesResponseType(typeof(PassengerDTO), 200)]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] string address)
        {
            try
            {
                var passenger = await _passengerService.UpdatePassengersAddress(id, address);
                if (passenger == null)
                {
                    _logger.LogError($"Passenger with id: {id} has not been found");
                    return NotFound();
                }
                _logger.LogInformation($"Returned updated Passenger details with id: {id}");
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateAddress action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("get/{id}/email/update")]
        [HttpPut("{id}/email/update")]
        [HttpPatch("get/{id}/email/update")]
        [HttpPatch("{id}/email/update")]
        //Endpoint that updates passenger’s email address
        [ProducesResponseType(typeof(PassengerDTO), 200)]
        public async Task<IActionResult> UpdateEmailAddress(int id, [FromBody] string emailAddress)
        {
            try
            {
                var passenger = await _passengerService.UpdatePassengersEmailAddress(id, emailAddress);
                if (passenger == null)
                {
                    _logger.LogError($"Passenger with id: {id} has not been found or e-mail address {emailAddress} is not valid");
                    return NotFound();
                }
                _logger.LogInformation($"Returned updated Passenger details with id: {id}");
                return Ok(passenger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateAddress action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
