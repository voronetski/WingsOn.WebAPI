using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{
    public class FlightAsyncRepository : FlightRepository, IAsyncRepository<Flight>
    {
        public async Task<IEnumerable<Flight>> GetAllAsync()
        {
            return await Task.FromResult(Repository.AsEnumerable());
        }

        public Task<Flight> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Flight element)
        {
            throw new NotImplementedException();
        }
    }
}
