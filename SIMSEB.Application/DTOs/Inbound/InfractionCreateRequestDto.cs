using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class InfractionCreateRequestDto
    {
        public Guid UserId { get; set; } // ID del usuario infractor
        public int TypeId { get; set; }  // ID del tipo de infracción (1, 2, 3)
    }

}
