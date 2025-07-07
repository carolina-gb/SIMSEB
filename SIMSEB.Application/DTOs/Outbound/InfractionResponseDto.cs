using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.Interfaces.Infractions;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class InfractionResponseDto
    {
        public Guid InfractionId { get; set; }
        public string InfractionNumber { get; set; }
        public UserDto User { get; set; } // Resumen del usuario infractor
        public InfractionTypeDto Type { get; set; } // Tipo de infracción
        public decimal Amount { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
