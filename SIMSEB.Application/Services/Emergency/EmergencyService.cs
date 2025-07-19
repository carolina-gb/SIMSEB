using Microsoft.AspNetCore.Http;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.Interfaces.Emergency;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Interfaces;

namespace SIMSEB.Application.Services.Emergency
{
    public class EmergencyType
    {
        public int Id { get; init; }
        public string Name { get; init; }        // Inglés
        public string ShowName { get; init; }    // Español
    }
    public class EmergencyService : IEmergencyService
    {
        private readonly IEmergencyRepository _emergencyRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmergencyService(
            IEmergencyRepository emergencyRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _emergencyRepository = emergencyRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<EmergencyResponseDto>> CreateAsync(CreateEmergencyRequestDto dto)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return new GeneralResponse<EmergencyResponseDto>
                    {
                        Code = 401,
                        Message = "No se pudo obtener el usuario desde el token.",
                        Data = null
                    };
                }

                var userId = Guid.Parse(userIdClaim);
                var user = await _userRepository.GetByIdAsync(userId);
                var emergency = new SIMSEB.Domain.Entities.Emergency
                {
                    TypeId = dto.TypeId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _emergencyRepository.AddAsync(emergency);
                var emergencyTypes = new List<EmergencyType>
                    {
                        new() { Id = 1, Name = "theft",  ShowName = "Robo"     },
                        new() { Id = 2, Name = "fire",   ShowName = "Incendio" },
                        new() { Id = 3, Name = "fight",  ShowName = "Pelea"    },
                    };
                var admins = await _userRepository.GetByTypeIdsAsync(new[] { 1, 2 });

                var notification = new NotificationMessage
                {
                    typeName = emergencyTypes
                  .FirstOrDefault(e => e.Id == dto.TypeId)?.ShowName
                  ?? "🤷‍♂️ Tipo de emergencia desconocido",
                    username = user.Username,
                    Message = "Nueva emergencia creada",
                    EmergencyId = emergency.EmergencyId,
                    CreatedAt = emergency.CreatedAt
                };

                await _notificationService.NotifyAdminsAsync(notification);

                return new GeneralResponse<EmergencyResponseDto>
                {
                    Code = 200,
                    Message = "Emergencia creada correctamente",
                    Data = new EmergencyResponseDto
                    {
                        EmergencyId = emergency.EmergencyId,
                        TypeId = emergency.TypeId,
                        UserId = emergency.UserId,
                        CreatedAt = emergency.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<EmergencyResponseDto>
                {
                    Code = 500,
                    Message = $"Ocurrió un error al crear la emergencia: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<EmergencyPaginatedResponseDto>> GetAllAsync(int skip)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
                var typeIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("typeId")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(typeIdClaim))
                {
                    return new GeneralResponse<EmergencyPaginatedResponseDto>
                    {
                        Code = 401,
                        Message = "Token inválido.",
                        Data = null
                    };
                }

                var userId = Guid.Parse(userIdClaim);
                var typeId = int.Parse(typeIdClaim);
                int take = 5;

                var emergencies = await _emergencyRepository.GetAllDetailedAsync(typeId, userId, skip, take);
                var count = await _emergencyRepository.CountAsync(typeId, userId);

                var response = new EmergencyPaginatedResponseDto
                {
                    Count = count,
                    Data = emergencies.Select(e => new EmergencyDetailedResponseDto
                    {
                        EmergencyId = e.EmergencyId,
                        TypeId = e.TypeId,
                        TypeName = e.Type.ShowName,
                        UserId = e.User.UserId,
                        Username = e.User.Username,
                        CreatedAt = e.CreatedAt
                    }).ToList()
                };

                return new GeneralResponse<EmergencyPaginatedResponseDto>
                {
                    Code = 200,
                    Message = "Emergencias obtenidas correctamente",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<EmergencyPaginatedResponseDto>
                {
                    Code = 500,
                    Message = $"Error al obtener emergencias: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
