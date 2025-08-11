using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.Interfaces.Reporting
{
    public interface IReportingService
    {
        Task<GeneralResponse<ReportingResponseDto>> GetReportAsync(ReportingRequest dto);
    }
}
