using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class UsersNotice
{
    public Guid UserOwnerId { get; set; }

    public Guid UserNoticeId { get; set; }

    public virtual User UserNotice { get; set; } = null!;

    public virtual User UserOwner { get; set; } = null!;
}
