using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class EmergencyPaginatedResponseDto
    {
        public int Count { get; set; }
        public List<EmergencyDetailedResponseDto> Data { get; set; } = new();
    }

    public class EmergencyDetailedResponseDto
    {
        public int EmergencyId { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
