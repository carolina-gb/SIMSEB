using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.Auth;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Domain.Utils;
using Microsoft.Extensions.Configuration;


namespace SIMSEB.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<GeneralResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var user = await _userRepository.GetByEmailOrUsernameAsync(request.Input);

                if (user == null)
                {
                    return new GeneralResponse<LoginResponseDto>
                    {
                        Code = 401,
                        Message = "Usuario no encontrado",
                        Data = null
                    };
                }

                var hashedInput = HashHelper.Hash(request.Password);

                if (user.Password != hashedInput)
                {
                    return new GeneralResponse<LoginResponseDto>
                    {
                        Code = 401,
                        Message = "Contraseña incorrecta",
                        Data = null
                    };
                }


                var secretKey = _configuration["Jwt:SecretKey"];
                var token = JwtHelper.GenerateToken(user, secretKey);

                return new GeneralResponse<LoginResponseDto>
                {
                    Code = 200,
                    Message = "Login exitoso",
                    Data = new LoginResponseDto
                    {
                        Token = token
                    }
                };

            }
            catch(Exception ex)
            {
                return new GeneralResponse<LoginResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno del servidor: {ex.Message}",
                    Data = null
                };
            }
            
        }
    }
}