using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SIMSEB.Domain.Entities
{
    public class User
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

        public  ICollection<Emergency> Emergencies { get; set; } = new List<Emergency>();
        public  ICollection<Infraction> Infractions { get; set; } = new List<Infraction>();

        public  ICollection<Report> ReportRejectByNavigations { get; set; } = new List<Report>();

        public  ICollection<Report> ReportUsers { get; set; } = new List<Report>();

        public  UsersStatus StatusNavigation { get; set; } = null!;

        public  UsersType Type { get; set; } = null!;
    }
}
