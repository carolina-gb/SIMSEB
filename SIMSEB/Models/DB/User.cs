using System;
using System.Collections.Generic;

namespace SIMSEB.Models.DB;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Identification { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int TypeId { get; set; }

    public int Status { get; set; }

    public string Password { get; set; } = null!;

    public string PasswordHint { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DeletedAt { get; set; }

    public virtual ICollection<Emergency> Emergencies { get; set; } = new List<Emergency>();

    public virtual ICollection<Infraction> Infractions { get; set; } = new List<Infraction>();

    public virtual ICollection<Report> ReportRejectByNavigations { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportUsers { get; set; } = new List<Report>();

    public virtual UsersStatus StatusNavigation { get; set; } = null!;

    public virtual UsersType Type { get; set; } = null!;
}
