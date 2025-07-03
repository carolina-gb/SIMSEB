using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class InfractionsType
    {
        public int InfractionTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Infraction> Infractions { get; set; } = new List<Infraction>();
    }
}
