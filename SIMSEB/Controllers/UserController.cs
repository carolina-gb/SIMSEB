using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.Interfaces.Auth;
using SIMSEB.Application.Interfaces.Users;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll([FromQuery] int skip = 0)
        {
            // Leer typeId desde el JWT
            var typeIdClaim = User.Claims.FirstOrDefault(c => c.Type == "typeId")?.Value;

            if (typeIdClaim == null || !int.TryParse(typeIdClaim, out int typeId))
                return Unauthorized(new { message = "No autorizado para acceder a esta información" });

            const int take = 5;

            var result = await _userService.GetVisibleUsersAsync(typeId, skip, take);
            return StatusCode(result.Code, result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var result = await _userService.CreateUserAsync(request);
            return StatusCode(result.Code, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto request)
        {
            var result = await _userService.UpdateUserAsync(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("get/by-id")]
        public async Task<IActionResult> GetById([FromQuery] Guid userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("get/by-username")]
        public async Task<IActionResult> GetByUsername([FromQuery] string username)
        {
            var result = await _userService.GetUserByUsernameAsync(username);
            return StatusCode(result.Code, result);
        }


    }
}
