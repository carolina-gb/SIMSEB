using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.UserManagement;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Domain.Utils;

namespace SIMSEB.Application.Services.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;

        public UserManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GeneralResponse<string>> ClearUserPasswordAsync(string targetUsername, string adminUsername)
        {
            try
            {
                var admin = await _userRepository.GetByEmailOrUsernameAsync(adminUsername);
                if (admin == null || admin.TypeId == 3) 
                    return new GeneralResponse<string>
                    {
                        Code = 401,
                        Message = "Solo un administrador puede realizar esta acción",
                        Data = null
                    };

                var user = await _userRepository.GetByEmailOrUsernameAsync(targetUsername);
                if (user == null)
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Usuario no encontrado",
                        Data = null
                    };

                var newPassword = PasswordGenerator.Generate(6);
                user.Password = HashHelper.Hash(newPassword);
                user.PasswordHint = HashHelper.Hash(newPassword);

                await _userRepository.UpdateAsync(user);

                return new GeneralResponse<string>
                {
                    Code = 200,
                    Message = "Contraseña restablecida correctamente",
                    Data = newPassword
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    Code = 500,
                    Message = $"Error interno del servidor: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
