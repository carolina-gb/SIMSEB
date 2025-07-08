using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.Emergency;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyController : ControllerBase
    {
        private readonly IEmergencyService _emergencyService;

        public EmergencyController(IEmergencyService emergencyService)
        {
            _emergencyService = emergencyService;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralResponse<object>>> CreateEmergency([FromBody] CreateEmergencyRequestDto dto)
        {
            var result = await _emergencyService.CreateAsync(dto);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResponse<EmergencyPaginatedResponseDto>>> GetAllEmergencies([FromQuery] int skip = 0)
        {
            var result = await _emergencyService.GetAllAsync(skip);
            return StatusCode(result.Code, result);
        }
    }
}
