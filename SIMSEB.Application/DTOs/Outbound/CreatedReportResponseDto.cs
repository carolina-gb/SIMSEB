using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class CreatedReportResponseDto
    {
        public Guid ReportId { get; set; }
        public string CaseNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CreatedBy { get; set; } = null!; 
        public string TypeName { get; set; } = null!;  
        public string StageName { get; set; } = null!; 
        public string EvidenceFileName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

}
