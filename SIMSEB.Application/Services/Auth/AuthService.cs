using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.Interfaces.Auth;

namespace SIMSEB.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Simulación de validación
            if (request.Email == "admin@simseb.com" && request.Password == "123456")
            {
                // Aquí deberías generar el JWT real
                return new LoginResponseDto
                {
                    Token = "fake-jwt-token",
                    Expiration = DateTime.UtcNow.AddHours(1)
                };
            }

            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }
    }
}