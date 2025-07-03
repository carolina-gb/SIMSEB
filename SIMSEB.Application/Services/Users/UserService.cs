using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.Users;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Interfaces;

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
    }
}
