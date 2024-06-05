using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddPersonInformation")]
        public async Task<IActionResult> AddPersonInformation([FromForm] PersonDto personDto, [FromForm] PlaceOfResidenceDto placeOfResidenceDto)
        {
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
                    filePath = await _personService.HandleFileUploadAsync(personDto.ProfilePhoto); // //Moved to helpers
                }

                await _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, filePath, birthDate);
                return Ok("Person information added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateName")]
        public async Task<IActionResult> UpdateName([FromForm] Guid personId, [FromForm] string newName) //what about Trim?
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest("Name cannot be blank or whitespace.");
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

        [HttpPut("UpdateLastName")]
        public async Task<IActionResult> UpdateLastName([FromQuery] Guid personId, [FromQuery] string newLastName)
        {
            if (string.IsNullOrWhiteSpace(newLastName))
            {
                return BadRequest("Name cannot be blank or whitespace.");
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

        [HttpPut("UpdateGender")]
        public async Task<IActionResult> UpdateGender([FromQuery] Guid personId, [FromQuery] string newGender)
        {
            if (string.IsNullOrWhiteSpace(newGender))
            {
                return BadRequest("Name cannot be blank or whitespace.");
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

        [HttpPut("UpdateBirthDate")]
        public async Task<IActionResult> UpdateBirthDate([FromQuery] Guid personId, [FromQuery] DateOnly newBirthDate)
        {
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

        [HttpPut("UpdatePersonalIdNumber")]
        public async Task<IActionResult> UpdateIdNumber([FromQuery] Guid personId, [FromQuery] string newPersonalIdNumber)
        {
            if (string.IsNullOrWhiteSpace(newPersonalIdNumber))
            {
                return BadRequest("ID number cannot be blank or whitespace.");
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

        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromQuery] Guid personId, [FromQuery] string newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                return BadRequest("Phone number cannot be blank or whitespace.");
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

        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromQuery] Guid personId, [FromQuery] string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                return BadRequest("Email cannot be blank or whitespace.");
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

        [HttpPut("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromQuery] Guid personId, [FromForm] IFormFile newProfilePhoto)
        {
            if (newProfilePhoto is null)
            {
                return BadRequest("Photo cannot be empty.");
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

        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromQuery] Guid personId, [FromQuery] string newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity))
            {
                return BadRequest("City cannot be blank or whitespace.");
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

        [HttpPut("UpdateStreet")]
        public async Task<IActionResult> UpdateStreet([FromQuery] Guid personId, [FromBody] string newStreet)
        {
            if (string.IsNullOrWhiteSpace(newStreet))
            {
                return BadRequest("Street cannot be blank or whitespace.");
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

        [HttpPut("UpdateHouseNumber")]
        public async Task<IActionResult> UpdateHouseNumber([FromQuery] Guid personId, [FromBody] int newHouseNumber)
        {
            if (newHouseNumber == 0)
            {
                return BadRequest("House number cannot be blank or whitespace.");
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

        [HttpPut("UpdateAppartmentNumber")]
        public async Task<IActionResult> UpdateAppartmentNumber([FromQuery] Guid personId, [FromBody] int newAppartmentNumber)
        {
            if (newAppartmentNumber == 0)
            {
                return BadRequest("Apartment number cannot be blank or whitespace.");
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

        [HttpGet("RetrievePerson")]

        //Get Downloadable photo of a person
    }
}