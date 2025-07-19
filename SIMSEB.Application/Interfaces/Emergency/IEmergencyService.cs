using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;

namespace SIMSEB.Application.Interfaces.Emergency
{
    public interface IEmergencyService
    {
        Task<GeneralResponse<EmergencyResponseDto>> CreateAsync(CreateEmergencyRequestDto dto);
        Task<GeneralResponse<EmergencyPaginatedResponseDto>> GetAllAsync(int skip);

    }
}
