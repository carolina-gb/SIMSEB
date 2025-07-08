using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class CreateReportRequestDto
    {
        public IFormFile EvidenceFile { get; set; } = null!; // PNG o JPG
        public string Description { get; set; } = null!;
        public int TypeId { get; set; } // 1: Ruido, 2: Suciedad (viene del frontend)
    }
}
