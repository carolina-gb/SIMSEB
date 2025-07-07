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

        public async Task<GeneralResponse<string>> ToggleInfractionStatusAsync(Guid infractionId)
        {
            try
            {
                // Obtener claims del usuario actual
                var userClaims = _httpContextAccessor.HttpContext?.User;
                var requesterTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (requesterTypeId != 1 && requesterTypeId != 2)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 401,
                        Message = "No tienes permisos para modificar el estado de esta infracción.",
                        Data = null
                    };
                }

                var infraction = await _infractionRepository.GetByIdAsync(infractionId);
                if (infraction == null)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Infracción no encontrada.",
                        Data = null
                    };
                }

                infraction.Active = !infraction.Active;
                infraction.UpdatedAt = DateTime.UtcNow;

                await _infractionRepository.UpdateAsync(infraction);

                return new GeneralResponse<string>
                {
                    Code = 200,
                    Message = infraction.Active ? "Infracción activada correctamente." : "Infracción inactivada correctamente.",
                    Data = infraction.InfractionId.ToString()
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

        public async Task<GeneralResponse<string>> UpdateInfractionTypeAsync(UpdateInfractionTypeRequestDto dto)
        {
            try
            {
                var userClaims = _httpContextAccessor.HttpContext?.User;
                var requesterTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (requesterTypeId != 1 && requesterTypeId != 2)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 401,
                        Message = "No tienes permisos para actualizar el tipo de infracción.",
                        Data = null
                    };
                }

                var infraction = await _infractionRepository.GetByIdAsync(dto.InfractionId);
                if (infraction == null)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Infracción no encontrada.",
                        Data = null
                    };
                }

                var infractionType = await _infractionTypeRepository.GetByIdAsync(dto.TypeId);
                if (infractionType == null)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Tipo de infracción no válido.",
                        Data = null
                    };
                }

                var newAmount = infractionType.Name switch
                {
                    "high" => 100.00m,
                    "moderate" => 50.00m,
                    "low" => 20.00m,
                    _ => 0m
                };

                infraction.TypeId = dto.TypeId;
                infraction.Amount = newAmount;
                infraction.UpdatedAt = DateTime.UtcNow;

                await _infractionRepository.UpdateAsync(infraction);

                return new GeneralResponse<string>
                {
                    Code = 200,
                    Message = "Tipo de infracción actualizado correctamente.",
                    Data = infraction.InfractionId.ToString()
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

        public async Task<GeneralResponse<InfractionResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {

                var userClaims = _httpContextAccessor.HttpContext?.User;
                var requesterTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (requesterTypeId != 1 && requesterTypeId != 2)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 401,
                        Message = "No tienes permisos para consultar esta infracción.",
                        Data = null
                    };
                }

                var infraction = await _infractionRepository.GetDetailedByIdAsync(id);
                if (infraction == null)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 404,
                        Message = "Infracción no encontrada.",
                        Data = null
                    };
                }

                var response = new InfractionResponseDto
                {
                    InfractionId = infraction.InfractionId,
                    InfractionNumber = infraction.InfractionNumber,
                    Amount = infraction.Amount,
                    Active = infraction.Active,
                    CreatedAt = infraction.CreatedAt,
                    User = new UserDto
                    {
                        UserId = infraction.User.UserId,
                        Username = infraction.User.Username,
                        FullName = $"{infraction.User.Name} {infraction.User.LastName}",
                        Identification = infraction.User.Identification,
                        Email = infraction.User.Email,
                        CreatedAt = infraction.User.CreatedAt,
                        UpdatedAt = infraction.User.UpdatedAt,
                        DeletedAt = infraction.User.DeletedAt,
                        Details = "Usuario con infracción",
                        UserStatus = infraction.User.StatusNavigation == null ? null : new UserStatus
                        {
                            UserStatusId = infraction.User.StatusNavigation.UserStatusId,
                            Name = infraction.User.StatusNavigation.Name,
                            ShowName = infraction.User.StatusNavigation.ShowName,
                            CreatedAt = infraction.User.StatusNavigation.CreatedAt
                        },
                        Type = infraction.User.Type == null ? null : new UserType
                        {
                            UserTypeId = infraction.User.Type.UserTypeId,
                            Name = infraction.User.Type.Name,
                            ShowName = infraction.User.Type.ShowName,
                            CreatedAt = infraction.User.Type.CreatedAt
                        }
                    },
                    Type = new InfractionTypeDto
                    {
                        InfractionTypeId = infraction.Type.InfractionTypeId,
                        Name = infraction.Type.Name,
                        ShowName = infraction.Type.ShowName,
                        CreatedAt = infraction.Type.CreatedAt
                    }
                };

                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 200,
                    Message = "Infracción obtenida correctamente.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<InfractionResponseDto>> GetByNumberAsync(string infractionNumber)
        {
            try
            {

                var userClaims = _httpContextAccessor.HttpContext?.User;
                var requesterTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (requesterTypeId != 1 && requesterTypeId != 2)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 401,
                        Message = "No tienes permisos para consultar esta infracción.",
                        Data = null
                    };
                }


                var infraction = await _infractionRepository.GetDetailedByNumberAsync(infractionNumber);
                if (infraction == null)
                {
                    return new GeneralResponse<InfractionResponseDto>
                    {
                        Code = 404,
                        Message = "Infracción no encontrada con ese número.",
                        Data = null
                    };
                }

                var response = new InfractionResponseDto
                {
                    InfractionId = infraction.InfractionId,
                    InfractionNumber = infraction.InfractionNumber,
                    Amount = infraction.Amount,
                    Active = infraction.Active,
                    CreatedAt = infraction.CreatedAt,
                    User = new UserDto
                    {
                        UserId = infraction.User.UserId,
                        Username = infraction.User.Username,
                        FullName = $"{infraction.User.Name} {infraction.User.LastName}",
                        Identification = infraction.User.Identification,
                        Email = infraction.User.Email,
                        CreatedAt = infraction.User.CreatedAt,
                        UpdatedAt = infraction.User.UpdatedAt,
                        DeletedAt = infraction.User.DeletedAt,
                        Details = "Usuario con infracción",
                        UserStatus = infraction.User.StatusNavigation == null ? null : new UserStatus
                        {
                            UserStatusId = infraction.User.StatusNavigation.UserStatusId,
                            Name = infraction.User.StatusNavigation.Name,
                            ShowName = infraction.User.StatusNavigation.ShowName,
                            CreatedAt = infraction.User.StatusNavigation.CreatedAt
                        },
                        Type = infraction.User.Type == null ? null : new UserType
                        {
                            UserTypeId = infraction.User.Type.UserTypeId,
                            Name = infraction.User.Type.Name,
                            ShowName = infraction.User.Type.ShowName,
                            CreatedAt = infraction.User.Type.CreatedAt
                        }
                    },
                    Type = new InfractionTypeDto
                    {
                        InfractionTypeId = infraction.Type.InfractionTypeId,
                        Name = infraction.Type.Name,
                        ShowName = infraction.Type.ShowName,
                        CreatedAt = infraction.Type.CreatedAt
                    }
                };

                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 200,
                    Message = "Infracción obtenida correctamente.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<InfractionResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<InfractionPaginatedResponseDto>> GetAllPaginatedAsync(int skip, int take)
        {
            try
            {
                var userClaims = _httpContextAccessor.HttpContext?.User;
                var requesterTypeId = int.Parse(userClaims?.FindFirst("typeId")?.Value ?? "0");

                if (requesterTypeId != 1 && requesterTypeId != 2)
                {
                    return new GeneralResponse<InfractionPaginatedResponseDto>
                    {
                        Code = 401,
                        Message = "No tienes permisos para consultar las infracciones.",
                        Data = null
                    };
                }

                var allInfractions = await _infractionRepository.GetAllDetailedAsync();

                var count = allInfractions.Count;
                var paginated = allInfractions.Skip(skip).Take(take);

                var data = paginated.Select(infraction => new InfractionResponseDto
                {
                    InfractionId = infraction.InfractionId,
                    InfractionNumber = infraction.InfractionNumber,
                    Amount = infraction.Amount,
                    Active = infraction.Active,
                    CreatedAt = infraction.CreatedAt,
                    User = new UserDto
                    {
                        UserId = infraction.User.UserId,
                        Username = infraction.User.Username,
                        FullName = $"{infraction.User.Name} {infraction.User.LastName}",
                        Identification = infraction.User.Identification,
                        Email = infraction.User.Email,
                        CreatedAt = infraction.User.CreatedAt,
                        UpdatedAt = infraction.User.UpdatedAt,
                        DeletedAt = infraction.User.DeletedAt,
                        Details = "Usuario con infracción",
                        UserStatus = infraction.User.StatusNavigation == null ? null : new UserStatus
                        {
                            UserStatusId = infraction.User.StatusNavigation.UserStatusId,
                            Name = infraction.User.StatusNavigation.Name,
                            ShowName = infraction.User.StatusNavigation.ShowName,
                            CreatedAt = infraction.User.StatusNavigation.CreatedAt
                        },
                        Type = infraction.User.Type == null ? null : new UserType
                        {
                            UserTypeId = infraction.User.Type.UserTypeId,
                            Name = infraction.User.Type.Name,
                            ShowName = infraction.User.Type.ShowName,
                            CreatedAt = infraction.User.Type.CreatedAt
                        }
                    },
                    Type = new InfractionTypeDto
                    {
                        InfractionTypeId = infraction.Type.InfractionTypeId,
                        Name = infraction.Type.Name,
                        ShowName = infraction.Type.ShowName,
                        CreatedAt = infraction.Type.CreatedAt
                    }
                }).ToList();

                return new GeneralResponse<InfractionPaginatedResponseDto>
                {
                    Code = 200,
                    Message = "Lista de infracciones obtenida correctamente.",
                    Data = new InfractionPaginatedResponseDto
                    {
                        Count = count,
                        Data = data
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<InfractionPaginatedResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}