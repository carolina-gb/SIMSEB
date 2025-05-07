using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class Report
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

    public virtual File EvidenceFile { get; set; } = null!;

    public virtual User? RejectByNavigation { get; set; }

    public virtual ICollection<ReportsTracking> ReportsTrackings { get; set; } = new List<ReportsTracking>();

    public virtual ReportsStage Stage { get; set; } = null!;

    public virtual ReportsType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
