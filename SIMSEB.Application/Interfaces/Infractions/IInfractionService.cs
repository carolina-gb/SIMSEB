using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;

namespace SIMSEB.Application.Interfaces.Infractions
{
    public interface IInfractionService
    {
        Task<GeneralResponse<InfractionResponseDto>> CreateInfractionAsync(InfractionCreateRequestDto dto);
        Task<GeneralResponse<string>> ToggleInfractionStatusAsync(Guid infractionId);
        Task<GeneralResponse<string>> UpdateInfractionTypeAsync(UpdateInfractionTypeRequestDto dto);
        Task<GeneralResponse<InfractionResponseDto>> GetByIdAsync(Guid id);
        Task<GeneralResponse<InfractionResponseDto>> GetByNumberAsync(string infractionNumber);
        Task<GeneralResponse<InfractionPaginatedResponseDto>> GetAllPaginatedAsync(int skip, int take);



    }
}
