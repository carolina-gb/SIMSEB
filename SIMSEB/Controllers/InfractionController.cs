using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.Interfaces.Infractions;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("infractions")]
    public class InfractionController : ControllerBase
    {
        private readonly IInfractionService _infractionService;

        public InfractionController(IInfractionService infractionService)
        {
            _infractionService = infractionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] InfractionCreateRequestDto dto)
        {
            var result = await _infractionService.CreateInfractionAsync(dto);
            return StatusCode(result.Code, result);
        }
    }
}