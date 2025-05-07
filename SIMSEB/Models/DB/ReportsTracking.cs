using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class ReportsTracking
{
    public int ReportTrackingId { get; set; }

    public Guid ReportId { get; set; }

    public int NewStageId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ReportsStage NewStage { get; set; } = null!;

    public virtual Report Report { get; set; } = null!;
}
