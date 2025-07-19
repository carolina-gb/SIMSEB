using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class Emergency
    {
        public int EmergencyId { get; set; }

        public int TypeId { get; set; }

        public Guid UserId { get; set; }

        [Column(TypeName = "numeric(9,6)")]
        public decimal? Latitude { get; set; }   // Usa ? si quieres permitir NULL

        [Column(TypeName = "numeric(9,6)")]
        public decimal? Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public EmergenciesType Type { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
