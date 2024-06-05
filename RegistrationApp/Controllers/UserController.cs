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

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromForm] UserDto userDto) //Should I implement password entry, which would be shown twice?
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
        public async Task<IActionResult> Login([FromForm] UserDto userDto) //check if [FromForm = separate fields for userName and Password
        {
            try
            {
                var user = await _userService.LoginAsync(userDto.Username, userDto.Password);
                var token = _jwtService.GetJwtToken(user.Id.ToString(), user.Username, user.Role);
                return Ok(new { user, token });
            }

            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
