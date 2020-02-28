using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Services
{
    public interface IBookingService
    {
        public Task<IEnumerable<PassengerDTO>> GetAllPassengersForFlight(string flightNumber);
        public Task<BookingDTO> AddPassengerToExistingFlight(string flightNumber, PassengerDTO passenger);
    }
}