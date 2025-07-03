using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class UsersStatus
    {
        public int UserStatusId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    
    }
}
