using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class InfractionPaginatedResponseDto
    {
        public int Count { get; set; }
        public List<InfractionResponseDto> Data { get; set; } = new();
    }

}
