using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Linq;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Services
{
    public class BookingService : IBookingService
    {
        private readonly IAsyncRepository<Booking> _bookingRepository;
        private readonly IAsyncRepository<Flight> _flightRepository;
        private readonly IAsyncRepository<Person> _personRepository;

        public BookingService(IAsyncRepository<Booking> bookingRepository, IAsyncRepository<Flight> flightRepository, IAsyncRepository<Person> personRepository)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        public async Task<IEnumerable<PassengerDTO>> GetAllPassengersForFlight(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber)) return null;

            var result = await _bookingRepository.GetAllAsync();
            var result1 = result.Where(booking => booking.Flight.Number == flightNumber)
                .SelectMany(booking => booking.ToBookingDto().Passengers);
            return await Task.FromResult(result1);
        }

        public async Task<BookingDTO> AddPassengerToExistingFlight(string flightNumber, PassengerDTO passenger)
        {
            if (string.IsNullOrWhiteSpace(flightNumber)) return null;
            if (passenger == null) return null;

            var allFlights = await _flightRepository.GetAllAsync();
            var newBookingFlight = allFlights.FirstOrDefault(flight =>
                string.Equals(flight.Number, flightNumber, StringComparison.CurrentCultureIgnoreCase));

            if (newBookingFlight == null) return null;

            await _personRepository.SaveAsync(passenger.ToPerson());

            var savedPerson = await _personRepository.GetAsync(passenger.Id);
            if (savedPerson == null) return null;

            var newBooking = new Booking
            {
                Flight = newBookingFlight,
                Customer = savedPerson,
                Passengers = new List<Person>{ savedPerson },
                Id = _bookingRepository.GetAllAsync().Result.Max(booking=>booking.Id) + 1,
                Number = "WO-999666",
                DateBooking = DateTime.Now,
            };

            await _bookingRepository.SaveAsync(newBooking);
            var result = await _bookingRepository.GetAsync(newBooking.Id);
            
            return await Task.FromResult(result.ToBookingDto());
        }
    }
}
