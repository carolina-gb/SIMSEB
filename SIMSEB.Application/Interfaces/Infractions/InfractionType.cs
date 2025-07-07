using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.Interfaces.Infractions
{
    public class InfractionTypeDto
    {
        public int InfractionTypeId { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
