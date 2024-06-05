using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image; /// --- check

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


                // Validate and parse the BirthDate - asking to input string, extra valiation will be added - should it go to services? //Ieally to be in services (services - ateina DTO ir iseina, Repositories - Entities
                if (!DateOnly.TryParseExact(personDto.BirthDate, "yyyy-MM-dd", out DateOnly birthDate))
                {
                    return BadRequest("Invalid date format for BirthDate. Please use YYYY-MM-DD.");
                }

                // Handle file upload - can it stay in the controller? - Move to services //Could be placed in helper
                string filePath = null;
                if (personDto.ProfilePhoto != null)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    filePath = Path.Combine(uploads, Guid.NewGuid().ToString() + Path.GetExtension(personDto.ProfilePhoto.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await personDto.ProfilePhoto.CopyToAsync(stream);
                    }

                    using (var image = Image.Load<Rgba32>(filePath))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(200, 200),
                            Mode = ResizeMode.Stretch
                        }));
                        image.Save(filePath); // Overwrite the original file
                    }
                }

                await _personService.AddPersonInformation(userId, personDto, placeOfResidenceDto, filePath, birthDate);
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

                await _personService.UpdateName(userId, personId, newName);
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

                await _personService.UpdateLastName(userId, personId, newLastName);
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

                await _personService.UpdateGender(userId, personId, newGender);
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

                await _personService.UpdateBirthDate(userId, personId, newBirthDate);
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

                await _personService.UpdateIdNumber(userId, personId, newPersonalIdNumber);
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

                await _personService.UpdatePhoneNumber(userId, personId, newPhoneNumber);
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

                await _personService.UpdatePhoneNumber(userId, personId, newEmail);
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

                await _personService.UpdatePhoto(userId, personId, newProfilePhoto);
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

                await _placeOfResidenceService.UpdateCity(userId, personId, newCity);
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

                await _placeOfResidenceService.UpdateCity(userId, personId, newStreet);
                return Ok("Street updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateHouseNumber")]
        public async Task<IActionResult> UpdateHouseNumber([FromQuery] Guid personId, [FromBody] string newHouseNumbe)
        {
            if (string.IsNullOrWhiteSpace(newHouseNumbe))
            {
                return BadRequest("House number cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateCity(userId, personId, newHouseNumbe);
                return Ok("House number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateAppartmentNumber")]
        public async Task<IActionResult> UpdateAppartmentNumber([FromQuery] Guid personId, [FromBody] string newAppartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(newAppartmentNumber))
            {
                return BadRequest("Apartment number cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await _placeOfResidenceService.UpdateCity(userId, personId, newAppartmentNumber);
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
                var personInfo = await _personService.RetrievePersonInformation(userId, personId);
                return Ok(personInfo);
            }
            catch (InvalidCastException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Get Downloadable photo of a person
        //Get all remaining info (except photo)
    }
}