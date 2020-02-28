using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.DTO;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<Booking> _logger;
        private readonly IBookingService _bookingService;

        public BookingsController(ILogger<Booking> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        [HttpGet("flights/get/{flightNumber}/passengers")]
        [HttpGet("flights/get/{flightNumber}/passengers/get")]
        [HttpGet("flights/{flightNumber}/passengers")]
        [HttpGet("flights/{flightNumber}/passengers/get")]
        [ProducesResponseType(typeof(IEnumerable<PassengerDTO>), 200)]
        // Endpoint that returns all passengers on the flight by the flight number
        public async Task<IActionResult> GetPassengersByFlight(string flightNumber)
        {
            try
            {
                var passengers = await _bookingService.GetAllPassengersForFlight(flightNumber);
                if (passengers != null) return Ok(passengers);

                _logger.LogError($"Passengers for flight number: {flightNumber} have not been found");
                return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetPassengersByFlight action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost("add/flights/{flightNumber}/passengers/add")]
        [HttpPost("add/flights/get/{flightNumber}/passengers/add")]
        // Endpoint that creates a booking of an existing flight for a new passenger
        // For testing purpose we make return type Booking instead of void
        [ProducesResponseType(typeof(BookingDTO), 200)]
        public async Task<IActionResult> CreateBooking(string flightNumber, [FromBody] PassengerDTO passenger)
        {
            try
            {
                var booking = await _bookingService.AddPassengerToExistingFlight(flightNumber, passenger);
                if (booking != null) return Ok(booking);

                _logger.LogError($"Either flight number: {flightNumber} was invalid or non-existing or something was wrong with a passenger to add");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateBooking action: {ex.Message}.");
                return StatusCode(500, "Something went wrong");
            }
        }

    }
}
