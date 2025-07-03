using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class File
    {
        public Guid FileId { get; set; }

        public string? Path { get; set; }

        public string Type { get; set; } = null!;

        public DateTime UploadedAt { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
