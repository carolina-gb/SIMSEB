using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class InfractionsType
{
    public int InfractionTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Infraction> Infractions { get; set; } = new List<Infraction>();
}
