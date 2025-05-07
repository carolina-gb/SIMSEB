using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class UsersStatus
{
    public int UserStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
