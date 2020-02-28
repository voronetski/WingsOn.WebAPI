using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{
    public interface IAsyncRepository<T> where T : DomainObject
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(int id);

        Task SaveAsync(T element);
    }
}
