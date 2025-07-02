using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class Report
    {
        public Guid ReportId { get; set; }

        public string? CaseNumber { get; set; }

        public int TypeId { get; set; }

        public Guid EvidenceFileId { get; set; }

        public Guid UserId { get; set; }

        public string Description { get; set; } = null!;

        public string? RejectReason { get; set; }

        public Guid? RejectBy { get; set; }

        public int StageId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public  File EvidenceFile { get; set; } = null!;

        public  User? RejectByNavigation { get; set; }

        public  ICollection<ReportsTracking> ReportsTrackings { get; set; } = new List<ReportsTracking>();

        public  ReportsStage Stage { get; set; } = null!;

        public  ReportsType Type { get; set; } = null!;

        public  User User { get; set; } = null!;
    }
}
