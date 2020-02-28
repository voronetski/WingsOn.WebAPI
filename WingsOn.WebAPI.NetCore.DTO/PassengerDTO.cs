using System;


namespace WingsOn.WebAPI.NetCore.DTO
{
    public class PassengerDTO : MainDTO
    {
        public string Name { get; set; }
        public string DateBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
