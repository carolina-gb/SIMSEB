using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class ReportsType
    {
        public int ReportTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
