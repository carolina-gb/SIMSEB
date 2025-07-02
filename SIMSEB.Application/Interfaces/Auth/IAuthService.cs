using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;

namespace SIMSEB.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<GeneralResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    }
}
