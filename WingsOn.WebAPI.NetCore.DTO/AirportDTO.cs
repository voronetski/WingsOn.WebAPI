using System;
using System.Collections.Generic;
using System.Text;

namespace WingsOn.WebAPI.NetCore.DTO
{
    public class AirportDTO : MainDTO
    {
        public string Code { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}
