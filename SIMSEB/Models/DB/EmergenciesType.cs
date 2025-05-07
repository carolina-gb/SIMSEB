using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class EmergenciesType
{
    public int EmergencyTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Emergency> Emergencies { get; set; } = new List<Emergency>();
}
