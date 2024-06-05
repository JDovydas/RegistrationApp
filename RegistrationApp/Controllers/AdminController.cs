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
        private readonly IPersonService _personService;

        public AdminController(IUserService userService, IPersonService personService)
        {
            _userService = userService;
            _personService = personService;
        }

        [HttpDelete("DeleteUserById")] /// Move to userControl
        public async Task<IActionResult> DeleteUser(Guid userId)
        //what should be the error catchings? Is the below enought?
        {
            try
            {
                await _userService.DeleteUserById(userId);
                return Ok("User deleted successfully");

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePersonById")]
        public async Task<IActionResult> DeletePerson(Guid personId)
        {
            try
            {
                await _personService.DeletePersonById(personId);
                return Ok("Person deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
