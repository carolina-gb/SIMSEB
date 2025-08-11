using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.Interfaces.Reporting;
using SIMSEB.Application.Services.Reporting;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportingController : Controller
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetReportAsync([FromQuery] ReportingRequest dto)
        {
            var response = await _reportingService.GetReportAsync(dto);
            return StatusCode(response.Code, response);
        }
    }
}
