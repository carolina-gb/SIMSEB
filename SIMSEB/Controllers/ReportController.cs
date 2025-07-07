using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.Interfaces.Reports;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize] // Requiere Bearer Token
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateReport([FromForm] CreateReportRequestDto dto)
        {
            var result = await _reportService.CreateReportAsync(dto);
            return StatusCode(result.Code, result);
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetByReportId(Guid reportId)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var typeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "typeId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId) || !int.TryParse(typeIdClaim, out var typeId))
                return Unauthorized("Token inválido.");

            var response = await _reportService.GetByReportIdAsync(reportId, userId, typeId);
            return StatusCode(response.Code, response);
        }

        [HttpGet("by-case-number")]
        public async Task<IActionResult> GetByCaseNumber([FromQuery] string caseNumber)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var typeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "typeId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId) || !int.TryParse(typeIdClaim, out var typeId))
                return Unauthorized("Token inválido.");

            var response = await _reportService.GetByCaseNumberAsync(userId, typeId, caseNumber);
            return StatusCode(response.Code, response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReports([FromQuery] int skip = 0)
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var typeIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "typeId")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId) || !int.TryParse(typeIdClaim, out var typeId))
                return Unauthorized("Token inválido.");

            var response = await _reportService.GetAllReportsPaginatedAsync(userId, typeId, skip);
            return StatusCode(response.Code, response);
        }


    }
}
