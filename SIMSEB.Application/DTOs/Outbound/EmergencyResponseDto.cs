using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class EmergencyResponseDto
    {
        public int EmergencyId { get; set; }  
        public int TypeId { get; set; }
        public Guid UserId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
