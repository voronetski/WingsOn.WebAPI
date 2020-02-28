using System;
using System.Collections.Generic;
using System.Text;

namespace WingsOn.WebAPI.NetCore.DTO
{
    public class FlightDTO : MainDTO
    {
        public string Number { get; set; }

        public AirlineDTO Carrier { get; set; }

        public AirportDTO DepartureAirport { get; set; }

        public string DepartureDate { get; set; }

        public AirportDTO ArrivalAirport { get; set; }

        public string ArrivalDate { get; set; }

        public decimal Price { get; set; }
    }
}
