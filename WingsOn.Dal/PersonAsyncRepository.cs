using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{

    public class PersonAsyncRepository : PersonRepository, IAsyncRepository<Person>
    {
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await Task.FromResult(Repository.AsEnumerable());
        }

        public async Task<Person> GetAsync(int id)
        {
            var result = Repository.Where(a => a.Id == id);
            return await Task.FromResult(result.FirstOrDefault());
        }

        public async Task SaveAsync(Person person)
        {
            if (person == null)
            {
                return;
            }

            var existing = await GetAsync(person.Id);
            if (existing != null)
            {
                Repository.Remove(existing);
            }

            Repository.Add(person);
        }
    }
}
