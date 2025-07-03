using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class ReportsStage
    {
        public int ReportStageId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public ICollection<ReportsTracking> ReportsTrackings { get; set; } = new List<ReportsTracking>();
    }
}
