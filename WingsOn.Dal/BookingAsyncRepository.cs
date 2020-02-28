using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{
    public class BookingAsyncRepository : BookingRepository, IAsyncRepository<Booking>
    {
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await Task.FromResult(Repository.AsEnumerable());
        }

        public async Task<Booking> GetAsync(int id)
        {
            var result = Repository.Where(a => a.Id == id);
            return await Task.FromResult(result.FirstOrDefault());
        }

        public async Task SaveAsync(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            var existing = await GetAsync(booking.Id);
            if (existing != null)
            {
                Repository.Remove(existing);
            }

            Repository.Add(booking);
        }
    }
}
