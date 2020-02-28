using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Services
{
    public interface IFlightService
    {
        public Task<FlightDTO> GetFlightByNumber(string flightNumber);
        public Task<IEnumerable<FlightDTO>> GetAllAsync();
    }
}