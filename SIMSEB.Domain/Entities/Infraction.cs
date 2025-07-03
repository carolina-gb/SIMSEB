using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class Infraction
    {
        public Guid InfractionId { get; set; }

        public string? InfractionNumber { get; set; }

        public Guid UserId { get; set; }

        public int TypeId { get; set; }

        public bool Active { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public InfractionsType Type { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
