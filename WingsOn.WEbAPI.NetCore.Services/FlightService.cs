using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Linq;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Services
{
    public class FlightService : IFlightService
    {
        private readonly IAsyncRepository<Flight> _repositoryAsync;

        public FlightService(IAsyncRepository<Flight> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync ?? throw new ArgumentNullException(nameof(repositoryAsync));
        }

        public async Task<IEnumerable<FlightDTO>> GetAllAsync()
        {
            var result = await _repositoryAsync.GetAllAsync();
            var result1 = result.Select(flight => flight.ToFlightDto());
            return await Task.FromResult(result1);
        }


        public async Task<FlightDTO> GetFlightByNumber(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber)) return null;

            var result = await _repositoryAsync.GetAllAsync();
            var result1 = result.FirstOrDefault(flight => flight.Number == flightNumber).ToFlightDto();

            return await Task.FromResult(result1);
        }
    }
}
