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
        // Private fields to hold service instances
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                //var token = _jwtService.GetJwtToken(user.Id.ToString(), user.Username, user.Role);
                // Returns 200 OK response with user information and token
                return Ok(new { user, token });
            }

            catch (InvalidOperationException ex)
            {
                // Catches any InvalidOperationException and returns 401 Unauthorized response with exception message
                return Unauthorized(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUserById")] /// Move to userControl
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        //what should be the error catchings? Is the below enought?
        {
            try
            {
                await _userService.DeleteUserByIdAsync(userId);
                // Returns 200 OK response with success message
                return Ok("User deleted successfully");

            }
            catch (InvalidOperationException ex)
            {
                // Catches any InvalidOperationException and returns 400 Bad Request response with the exception message
                return BadRequest(ex.Message);
            }
        }
    }
}
