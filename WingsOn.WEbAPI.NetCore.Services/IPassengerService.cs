using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.WebAPI.NetCore.DTO;

namespace WingsOn.WebAPI.NetCore.Services
{
    public interface IPassengerService
    {
        public Task<IEnumerable<PassengerDTO>> GetAll();
        public Task<PassengerDTO> GetPassengerById(int id);
        public Task<IEnumerable<PassengerDTO>> GetByGender(string gender);
        public Task<PassengerDTO> UpdatePassengersAddress(int id, string address);
        public Task<PassengerDTO> UpdatePassengersEmailAddress(int id, string emailAddress);
        public Task Save(PassengerDTO passenger);
    }
}