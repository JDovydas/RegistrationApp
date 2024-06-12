using RegistrationApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Services.Interfaces;

namespace RegistrationApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        // Constructor - initializes services via dependency injection
        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromForm] UserDto userDto)
        {
            try
            {
                var user = await _userService.SignUpAsync(userDto.Username, userDto.Password);
                return Ok("User created successfully");
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserDto userDto)
        {
            try
            {
                var user = await _userService.LoginAsync(userDto.Username, userDto.Password);

                // Generates JWT token for the authenticated user
                var token = _jwtService.GetJwtToken(user.Id.ToString(), user.Role);

                // Returns 200 OK response with token
                return Ok(token);
            }

            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUserById")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                await _userService.DeleteUserByIdAsync(userId);

                return Ok("User deleted successfully");

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
