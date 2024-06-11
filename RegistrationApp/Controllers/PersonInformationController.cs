using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Shared.DTOs;
using System.Security.Claims;

namespace RegistrationApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonInformationController : ControllerBase
    {
        // Hold service instance
        private readonly IPersonService _personService;

        // Constructor - initializes service via dependency injection
        public PersonInformationController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddPersonInformation")]
        public async Task<IActionResult> AddPersonInformation([FromForm] PersonDto personDto, [FromForm] PlaceOfResidenceDto placeOfResidenceDto)
        {
            try
            {
                // Get user ID from HTTP context
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Validate birth date format
                if (!_personService.ValitateBirthDate(personDto.BirthDate, out DateOnly birthDate))
                {
                    return BadRequest("Invalid date format for BirthDate. Please use YYYY-MM-DD.");
                }

                // Handle profile photo
                string filePath = null;
                if (personDto.ProfilePhoto != null)
                {
                    filePath = await _personService.UploadProfilePhotoAsync(personDto.ProfilePhoto);
                }

                // Add person information
                await _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, filePath, birthDate);
                return Ok("Person information added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdatePersonInformation")]
        public async Task<IActionResult> UpdatePersonInformation([FromForm] Guid personId, [FromForm] UpdatePersonDto personDto, [FromForm] UpdatePlaceOfResidenceDto placeOfResidenceDto)
        {
            try
            {
                // Get user ID from HTTP context
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Validate birth date format
                if (!_personService.ValitateBirthDate(personDto.BirthDate, out DateOnly birthDate))//Moved to helpers
                {
                    return BadRequest("Invalid date format for BirthDate. Please use YYYY-MM-DD.");
                }

                // Handle profile photo
                string filePath = null;
                if (personDto.ProfilePhoto != null)
                {
                    filePath = await _personService.UploadProfilePhotoAsync(personDto.ProfilePhoto); //Moved to helpers
                }

                // Add person information
                await _personService.UpdatePersonInformationAsync(personId, userId, personDto, placeOfResidenceDto, filePath, birthDate);
                return Ok("Person information updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("RetrieveAllPersonInformation")]
        public async Task<IActionResult> RetrievePersonInformation([FromQuery] Guid personId)
        {
            try
            {
                // Get user ID from HTTP context
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var personInfo = await _personService.RetrievePersonInformationAsync(userId, personId);
                return Ok(personInfo);
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("RetrievePersonProfilePhoto")]
        public async Task<IActionResult> RetrievePersonProfilePhoto([FromQuery] Guid personId)
        {
            try
            {
                // Get user ID from HTTP context
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var personProfilePhoto = await _personService.RetrievePersonProfilePhotoAsync(userId, personId);
                return personProfilePhoto;
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePersonById")]
        public async Task<IActionResult> DeletePersonAsync(Guid personId)
        {
            try
            {
                await _personService.DeletePersonByIdAsync(personId);
                return Ok("Person deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}



