using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class EmergenciesType
    {
        public int EmergencyTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Emergency> Emergencies { get; set; } = new List<Emergency>();
    }
}
