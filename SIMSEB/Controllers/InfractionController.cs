using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.Interfaces.Infractions;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    

        [HttpPut("toggle/{id}")]
        [Authorize]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _infractionService.ToggleInfractionStatusAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpPut("update-type")]
        [Authorize]
        public async Task<IActionResult> UpdateType([FromBody] UpdateInfractionTypeRequestDto dto)
        {
            var result = await _infractionService.UpdateInfractionTypeAsync(dto);
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _infractionService.GetByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpGet("by-number/{infractionNumber}")]
        [Authorize]
        public async Task<IActionResult> GetByNumber(string infractionNumber)
        {
            var result = await _infractionService.GetByNumberAsync(infractionNumber);
            return StatusCode(result.Code, result);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int skip)
        {
            const int take = 5;
            var result = await _infractionService.GetAllPaginatedAsync(skip, take);
            return StatusCode(result.Code, result);
        }


    }
}