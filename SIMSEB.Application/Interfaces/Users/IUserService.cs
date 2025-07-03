using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<GeneralResponse<UserPaginatedResponseDto>> GetVisibleUsersAsync(int typeId, int take, int skip);
    }
}
