using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class File
{
    public Guid FileId { get; set; }

    public string? Path { get; set; }

    public string Type { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
