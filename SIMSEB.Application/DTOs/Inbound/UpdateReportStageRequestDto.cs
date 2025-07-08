using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class UpdateReportStageRequestDto
    {
        public Guid ReportId { get; set; }
        public int StageId { get; set; }
        public string? RejectReason { get; set; }
    }

}
