using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Outbound.Response;

namespace SIMSEB.Application.Interfaces.UserManagement
{
    public interface IUserManagementService
    {
        Task<GeneralResponse<string>> ClearUserPasswordAsync(string targetUsername, string adminUsername);
        Task<GeneralResponse<string>> ChangePasswordAsync(string username, string currentPassword, string newPassword);

    }
}
