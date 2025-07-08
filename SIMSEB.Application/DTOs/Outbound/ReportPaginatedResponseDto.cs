using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class ReportPaginatedResponseDto
    {
        public int Count { get; set; }
        public List<ReportDto> Data { get; set; } = new();
    }

}
