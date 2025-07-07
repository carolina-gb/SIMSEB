using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;

namespace SIMSEB.Application.Interfaces.Reports
{
    public interface IReportService
    {
        Task<GeneralResponse<CreatedReportResponseDto>> CreateReportAsync(CreateReportRequestDto dto);
        Task<GeneralResponse<ReportListByUserIdResponseDto>> GetAllByUserIdAsync(Guid userId);
        Task<GeneralResponse<ReportListByUserIdResponseDto>> GetByCaseNumberAsync(Guid userId, int typeId, string caseNumber);
        Task<GeneralResponse<ReportPaginatedResponseDto>> GetAllReportsPaginatedAsync(Guid userId, int typeId, int skip);
        Task<GeneralResponse<ReportDto>> GetByReportIdAsync(Guid reportId, Guid userId, int typeId);
        Task<GeneralResponse<string>> UpdateStageAsync(UpdateReportStageRequestDto dto, Guid userId, int typeId);



    }
}
