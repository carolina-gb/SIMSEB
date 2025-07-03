using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class ReportsTracking
    {
        public int ReportTrackingId { get; set; }

        public Guid ReportId { get; set; }

        public int NewStageId { get; set; }

        public DateTime CreatedAt { get; set; }

        public ReportsStage NewStage { get; set; } = null!;

        public Report Report { get; set; } = null!;
    }
}
