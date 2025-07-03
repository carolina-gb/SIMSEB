using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Domain.Entities
{
    public class UsersNotice
    {
        public Guid UserOwnerId { get; set; }

        public Guid UserNoticeId { get; set; }

        public User UserNotice { get; set; } = null!;

        public User UserOwner { get; set; } = null!;
    }
}
