using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Shared.DTOs;
using System.Security.Claims;

namespace RegistrationApp.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonInformationController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IPlaceOfResidenceService _placeOfResidenceService;
        private readonly IUserService _userService;
        public PersonInformationController(IPersonService personService, IPlaceOfResidenceService placeOfResidenceService, IUserService userService)
        {
            _personService = personService;
            _placeOfResidenceService = placeOfResidenceService;
            _userService = userService;

        }

        [Authorize(Roles = "User")]
        [HttpPost("AddPersonInformation")]
        public async Task<IActionResult> AddPersonInformation([FromForm] PersonDto personDto, [FromForm] PlaceOfResidenceDto placeOfResidenceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (!_personService.ValitateBirthDate(personDto.BirthDate, out DateOnly birthDate))//Moved to helpers
                {
                    return BadRequest("Invalid date format for BirthDate. Please use YYYY-MM-DD.");
                }

                string filePath = null;
                if (personDto.ProfilePhoto != null)
                {
                    filePath = await _personService.ProfilePhotoUploadAsync(personDto.ProfilePhoto); // //Moved to helpers
                }

                await _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, filePath, birthDate);
                return Ok("Person information added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateName")]
        public async Task<IActionResult> UpdateName([FromForm] Guid personId, [FromForm] string newName) //what about Trim?
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdateNameAsync(userId, personId, newName);
                return Ok("Name updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateLastName")]
        public async Task<IActionResult> UpdateLastName([FromQuery] Guid personId, [FromQuery] string newLastName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdateLastNameAsync(userId, personId, newLastName);
                return Ok("Last name updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateGender")]
        public async Task<IActionResult> UpdateGender([FromQuery] Guid personId, [FromQuery] string newGender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdateGenderAsync(userId, personId, newGender);
                return Ok("Gender updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateBirthDate")]
        public async Task<IActionResult> UpdateBirthDate([FromQuery] Guid personId, [FromQuery] DateOnly newBirthDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdateBirthDateAsync(userId, personId, newBirthDate);
                return Ok("Birth date updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdatePersonalIdNumber")]
        public async Task<IActionResult> UpdateIdNumber([FromQuery] Guid personId, [FromQuery] string newPersonalIdNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdateIdNumberAsync(userId, personId, newPersonalIdNumber);
                return Ok("ID number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromQuery] Guid personId, [FromQuery] string newPhoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdatePhoneNumberAsync(userId, personId, newPhoneNumber);
                return Ok("Phone number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromQuery] Guid personId, [FromQuery] string newEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdatePhoneNumberAsync(userId, personId, newEmail);
                return Ok("Email updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromQuery] Guid personId, [FromForm] IFormFile newProfilePhoto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _personService.UpdatePhotoAsync(userId, personId, newProfilePhoto);
                return Ok("Photo updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromQuery] Guid personId, [FromQuery] string newCity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateCityAsync(userId, personId, newCity);
                return Ok("City updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateStreet")]
        public async Task<IActionResult> UpdateStreet([FromQuery] Guid personId, [FromBody] string newStreet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateStreetAsync(userId, personId, newStreet);
                return Ok("Street updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateHouseNumber")]
        public async Task<IActionResult> UpdateHouseNumber([FromQuery] Guid personId, [FromBody] int newHouseNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateHouseNumberAsync(userId, personId, newHouseNumber);
                return Ok("House number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateAppartmentNumber")]
        public async Task<IActionResult> UpdateAppartmentNumber([FromQuery] Guid personId, [FromBody] int newAppartmentNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateAppartmentNumberAsync(userId, personId, newAppartmentNumber);
                return Ok("Apartment number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "User")]
        [HttpGet("RetrieveAllInformation")]
        public async Task<IActionResult> RetrievePersonInformation([FromQuery] Guid personId)
        {
            try
            {
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



