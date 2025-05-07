using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class Emergency
{
    public int EmergencyId { get; set; }

    public int TypeId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual EmergenciesType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
