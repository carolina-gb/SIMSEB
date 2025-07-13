using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class NotificationMessage
    {
        public string typeName { get; set; }

        public string username { get; set; }
        public string Message { get; set; }
        public int EmergencyId { get; set; }  
        public DateTime CreatedAt { get; set; }
    }

}
