using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Linq;
using WingsOn.WebAPI.NetCore.DTO;


namespace WingsOn.WebAPI.NetCore.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IAsyncRepository<Person> _repositoryAsync;

        public PassengerService(IAsyncRepository<Person> repositoryAsync)
        {
            _repositoryAsync = repositoryAsync ?? throw new ArgumentNullException(nameof(repositoryAsync));
        }

        public async Task<IEnumerable<PassengerDTO>> GetAll()
        {
            var result = await _repositoryAsync.GetAllAsync();
            var result1 = result.Select(person => person.ToPassengerDto());
            return await Task.FromResult(result1);
        }

        public async Task<IEnumerable<PassengerDTO>> GetByGender(string gender)
        {
            if (!Enum.TryParse(gender, true, out GenderType _gender)) return null;
            var result = await _repositoryAsync.GetAllAsync();
            return await Task.FromResult(result.Where(person => person.Gender == _gender).Select(person => person.ToPassengerDto()));
        }

        public async Task<PassengerDTO> GetPassengerById(int id)
        {
            if (id <= 0) return null;
            var result = await _repositoryAsync.GetAsync(id);
            var result1 = result.ToPassengerDto();
            return await Task.FromResult(result1);
        }

        public async Task<PassengerDTO> UpdatePassengersEmailAddress(int id, string emailAddress)
        {
            var passenger = await GetPassengerById(id);
            if (passenger == null) return null;

            if (string.IsNullOrWhiteSpace(emailAddress)) return null;
            try
            {
                var validEmailAddress = new System.Net.Mail.MailAddress(emailAddress).Address;
                if (!string.Equals(emailAddress, validEmailAddress, StringComparison.CurrentCultureIgnoreCase))
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }

            passenger.Email = emailAddress;

            await Save(passenger);
            return await GetPassengerById(id);
        }

        public async Task Save(PassengerDTO passenger)
        {
            if (passenger == null) throw new ArgumentNullException();
            await _repositoryAsync.SaveAsync(passenger.ToPerson());
        }

        public async Task<PassengerDTO> UpdatePassengersAddress(int id, string address)
        {
            var passenger = await GetPassengerById(id);
            if (passenger == null) return null;

            passenger.Address = address;

            await Save(passenger);
            return await GetPassengerById(id);
        }
    }
}
