using RegistrationApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using System.Security.Claims;

namespace RegistrationApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IPersonService _personService;

        public AdminController(IUserService userService, IJwtService jwtService, IPersonService personService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _personService = personService;
        }

        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult<User>> DeleteUser(Guid userId)
        //what should be the error catchings? Is the below enought?
        //Is it ok that "Unauthorized" is cought in one way and then "Catch" exception is done slightly dfferently?
        {
            try
            {
                var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "Admin")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _userService.DeleteUserById(userId, username, userRole);
                return Ok("User deleted successfully");

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex) // should I leave this since I catch "Unauthorized" above?
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
