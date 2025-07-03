using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class UserPaginatedResponseDto
    {
        public int Count { get; set; }
        public List<UserDto> Data { get; set; } = new();
    }

    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserStatus UserStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Details { get; set; } = null!;
        public UserType Type { get; set; } = null!;

    }

    public class UserStatus
    {
        public int UserStatusId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }

    public class UserType
    {
        public int UserTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string ShowName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
