using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class Infraction
{
    public Guid InfractionId { get; set; }

    public string? InfractionNumber { get; set; }

    public Guid UserId { get; set; }

    public int TypeId { get; set; }

    public bool Active { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual InfractionsType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
