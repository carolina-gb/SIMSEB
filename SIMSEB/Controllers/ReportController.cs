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
    }
}
