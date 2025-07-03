using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.Users;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Domain.Utils;

namespace SIMSEB.Application.Services.Users
{
    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<GeneralResponse<UserPaginatedResponseDto>> GetVisibleUsersAsync(int requesterTypeId, int skip, int take)
        {
            try
            {
                IEnumerable<User> allUsers = Enumerable.Empty<User>();

                if (requesterTypeId == 1)
                {
                    allUsers = await _userRepository.GetByTypeIdsAsync(new[] { 2, 3 });
                }
                else if (requesterTypeId == 2)
                {
                    allUsers = await _userRepository.GetByTypeIdsAsync(new[] { 3 });
                }

                var count = allUsers.Count();
                var paginated = allUsers.Skip(skip).Take(take);

                var mappedUsers = paginated.Select(user => new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = $"{user.Name} {user.LastName}",
                    Identification = user.Identification,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    DeletedAt = user.DeletedAt,
                    Details = "",
                    UserStatus = user.StatusNavigation == null ? null : new UserStatus
                    {
                        UserStatusId = user.StatusNavigation.UserStatusId,
                        Name = user.StatusNavigation.Name,
                        ShowName = user.StatusNavigation.ShowName,
                        CreatedAt = user.StatusNavigation.CreatedAt
                    },
                    Type = user.Type == null ? null : new UserType
                    {
                        UserTypeId = user.Type.UserTypeId,
                        Name = user.Type.Name,
                        ShowName = user.Type.ShowName,
                        CreatedAt = user.Type.CreatedAt
                    }
                });

                return new GeneralResponse<UserPaginatedResponseDto>
                {
                    Code = 200,
                    Message = "Lista de usuarios obtenida correctamente",
                    Data = new UserPaginatedResponseDto
                    {
                        Count = count,
                        Data = mappedUsers.ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserPaginatedResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<CreatedUserResponseDto>> CreateUserAsync(CreateUserRequestDto request)
        {
            try
            {
                // Verificar si el username ya existe
                var existingUser = await _userRepository.GetByEmailOrUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    return new GeneralResponse<CreatedUserResponseDto>
                    {
                        Code = 409,
                        Message = "El nombre de usuario ya existe. Por favor intente con otro.",
                        Data = null
                    };
                }

                if (request.TypeId != 2 && request.TypeId != 3)
                {
                    return new GeneralResponse<CreatedUserResponseDto>
                    {
                        Code = 400,
                        Message = "Tipo de usuario inválido. Solo se permiten tipo 2 o 3.",
                        Data = null
                    };
                }

                var plainPassword = PasswordGenerator.Generate(6);
                var hashedPassword = HashHelper.Hash(plainPassword);

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = request.Username,
                    Name = request.Name,
                    LastName = request.LastName,
                    Identification = request.Identification,
                    Email = request.Email,
                    TypeId = request.TypeId,
                    Status = 1, // siempre 1
                    Password = hashedPassword,
                    PasswordHint = hashedPassword,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);

                return new GeneralResponse<CreatedUserResponseDto>
                {
                    Code = 201,
                    Message = "Usuario creado exitosamente",
                    Data = new CreatedUserResponseDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        Password = plainPassword
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CreatedUserResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<string>> UpdateUserAsync(UpdateUserRequestDto request)
        {
            try
            {
                if (request.TypeId != 2 && request.TypeId != 3)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 400,
                        Message = "El tipo de usuario debe ser 2 (Admin) o 3 (Usuario).",
                        Data = null
                    };
                }

                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Usuario no encontrado.",
                        Data = null
                    };
                }

                user.Username = request.Username;
                user.Name = request.Name;
                user.LastName = request.LastName;
                user.Identification = request.Identification;
                user.Email = request.Email;
                user.TypeId = request.TypeId;
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                return new GeneralResponse<string>
                {
                    Code = 200,
                    Message = "Usuario actualizado correctamente.",
                    Data = user.UserId.ToString()
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<UserPaginatedResponseDto>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new GeneralResponse<UserPaginatedResponseDto>
                    {
                        Code = 404,
                        Message = "Usuario no encontrado.",
                        Data = null
                    };
                }

                var dto = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = $"{user.Name} {user.LastName}",
                    Identification = user.Identification,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    DeletedAt = user.DeletedAt,
                    Details = "",
                    UserStatus = user.StatusNavigation == null ? null! : new UserStatus
                    {
                        UserStatusId = user.StatusNavigation.UserStatusId,
                        Name = user.StatusNavigation.Name,
                        ShowName = user.StatusNavigation.ShowName,
                        CreatedAt = user.StatusNavigation.CreatedAt
                    },
                    Type = user.Type == null ? null! : new UserType
                    {
                        UserTypeId = user.Type.UserTypeId,
                        Name = user.Type.Name,
                        ShowName = user.Type.ShowName,
                        CreatedAt = user.Type.CreatedAt
                    }
                };

                return new GeneralResponse<UserPaginatedResponseDto>
                {
                    Code = 200,
                    Message = "Usuario obtenido correctamente.",
                    Data = new UserPaginatedResponseDto
                    {
                        Count = 1,
                        Data = new List<UserDto> { dto }
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<UserPaginatedResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }



    }
}
