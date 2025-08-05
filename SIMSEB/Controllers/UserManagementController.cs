using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.UserManagement;

namespace SIMSEB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpPut("clear-password")]
        public async Task<IActionResult> ClearPassword([FromQuery] string targetUsername, [FromQuery] string adminUsername)
        {
            try
            {
                var response = await _userManagementService.ClearUserPasswordAsync(targetUsername, adminUsername);

                return response.Code switch
                {
                    200 => Ok(response),
                    401 => Unauthorized(response),
                    404 => NotFound(response),
                    _ => StatusCode(500, response)
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new GeneralResponse<string>
                {
                    Code = 401,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new GeneralResponse<string>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse<string>
                {
                    Code = 500,
                    Message = $"Error interno del servidor: {ex.Message}",
                    Data = null
                });
            }
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Unauthorized("Token de portador no encontrado.");
                }

                if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized("Formato de token no válido. Se espera un token de tipo 'Bearer'.");
                }
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    return Unauthorized();
                }

                var claims = jsonToken.Claims;
                var userIdClaim = claims.FirstOrDefault(c => c.Type == "userId")?.Value;

                Guid.TryParse(userIdClaim, out Guid userId);

                var result = await _userManagementService.ChangePasswordAsync(
                    userId,
                    request.CurrentPassword,
                    request.NewPassword
                );

                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse<string>
                {
                    Code = 500,
                    Message = $"Error interno del servidor: {ex.Message}",
                    Data = null
                });
            }
        }

    }
}
