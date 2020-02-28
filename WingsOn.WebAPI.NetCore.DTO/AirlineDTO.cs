using System;
using System.Collections.Generic;
using System.Text;

namespace WingsOn.WebAPI.NetCore.DTO
{
    public class AirlineDTO : MainDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }
}
