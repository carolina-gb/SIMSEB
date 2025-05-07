using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class ReportsType
{
    public int ReportTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
