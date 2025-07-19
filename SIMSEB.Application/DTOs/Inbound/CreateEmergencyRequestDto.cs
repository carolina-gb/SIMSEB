using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class CreateEmergencyRequestDto
    {
        public int TypeId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
