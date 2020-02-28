using System;
using System.Collections.Generic;
using System.Text;

namespace WingsOn.WebAPI.NetCore.DTO
{
    public class BookingDTO : MainDTO
    {
        public string Number { get; set; }

        public FlightDTO Flight { get; set; }

        public PassengerDTO Customer { get; set; }

        public IEnumerable<PassengerDTO> Passengers { get; set; }

        public string DateBooking { get; set; }
    }
}
