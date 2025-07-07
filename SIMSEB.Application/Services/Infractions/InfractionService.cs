using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.Interfaces.Infractions;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Interfaces;

namespace SIMSEB.Application.Services.Infractions
{
    public class InfractionService : IInfractionService
    {
        private readonly IInfractionRepository _infractionRepository;
        private readonly IInfractionTypeRepository _infractionTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InfractionService(
            IInfractionRepository infractionRepository,
            IInfractionTypeRepository infractionTypeRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _infractionRepository = infractionRepository;
            _infractionTypeRepository = infractionTypeRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<InfractionResponseDto>> CreateInfractionAsync(InfractionCreateRequestDto dto)
        {
            try
            {
                var userClaims = _httpContextAccessor.HttpContext?.User;
                var creatorTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (creatorTypeId != 1 && creatorTypeId != 2)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 401,
                        Message = "No estás autorizado para crear infracciones.",
                        Data = null
                    };
                }

                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 404,
                        Message = "Usuario infractor no encontrado.",
                        Data = null
                    };
                }

                var infractionType = await _infractionTypeRepository.GetByIdAsync(dto.TypeId);
                if (infractionType == null)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 404,
                        Message = "Tipo de infracción no encontrado.",
                        Data = null
                    };
                }

                var amount = infractionType.Name switch
                {
                    "high" => 100.00m,
                    "moderate" => 50.00m,
                    "low" => 20.00m,
                    _ => 0m
                };

                var infractionNumber = await _infractionRepository.GenerateNextInfractionNumberAsync();

                var infraction = new Infraction
                {
                    InfractionId = Guid.NewGuid(),
                    InfractionNumber = infractionNumber,
                    UserId = dto.UserId,
                    TypeId = dto.TypeId,
                    Active = true,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _infractionRepository.CreateAsync(infraction);

                var response = new InfractionResponseDto
                {
                    InfractionId = infraction.InfractionId,
                    InfractionNumber = infraction.InfractionNumber,
                    Amount = infraction.Amount,
                    Active = infraction.Active,
                    CreatedAt = infraction.CreatedAt,
                    User = new UserDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        FullName = $"{user.Name} {user.LastName}",
                        Identification = user.Identification,
                        Email = user.Email,
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
                        },
                        Details = "Usuario con infracción recibida"
                    },
                    Type = new InfractionTypeDto
                    {
                        InfractionTypeId = infractionType.InfractionTypeId,
                        Name = infractionType.Name,
                        ShowName = infractionType.ShowName,
                        CreatedAt = infractionType.CreatedAt
                    }
                };

                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 201,
                    Message = "Infracción creada exitosamente.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno al crear la infracción: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}