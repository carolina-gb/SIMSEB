using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class UpdateInfractionTypeRequestDto
    {
        public Guid InfractionId { get; set; }
        public int TypeId { get; set; }
    }
}
