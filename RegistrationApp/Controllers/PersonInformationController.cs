using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;
using System.Text.Json;

namespace RegistrationApp.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonInformationController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IPlaceOfResidenceService _placeOfResidenceService;
        public PersonInformationController(IPersonService personService, IPlaceOfResidenceService placeOfResidenceService)
        {
            _personService = personService;
            _placeOfResidenceService = placeOfResidenceService;
        }

        [HttpPost("AddPersonInformation")]
        public async Task<IActionResult> AddPersonInformation([FromForm] PersonDto personDto, [FromForm] PlaceOfResidenceDto placeOfResidenceDto)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }

                // Validate and parse the BirthDate
                if (!DateOnly.TryParseExact(personDto.BirthDate, "yyyy-MM-dd", out DateOnly birthDate))
                {
                    return BadRequest("Invalid date format for BirthDate. Please use YYYY-MM-DD.");
                }

                // Handle file upload
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
        public async Task<IActionResult> UpdateName([FromQuery] Guid personId, [FromBody] string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest("Name cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdateName(userId, personId, newName);
                return Ok("Name updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("UpdateLastName")]
        public async Task<IActionResult> UpdateLastName([FromQuery] Guid personId, [FromBody] string newLastName)
        {
            if (string.IsNullOrWhiteSpace(newLastName))
            {
                return BadRequest("Name cannot be blank or whitespace.");
            }

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                //await _personService.UpdateLastName(username, personId, newLastName);
                await _personService.UpdateLastName(userId, personId, newLastName);
                return Ok("Name updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateGender")]
        public async Task<IActionResult> UpdateGender([FromQuery] Guid personId, [FromBody] string newGender)
        {
            if (string.IsNullOrWhiteSpace(newGender))
            {
                return BadRequest("Name cannot be blank or whitespace.");
            }

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdateGender(userId, personId, newGender);
                return Ok("Gender updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateBirthDate")]
        public async Task<IActionResult> UpdateBirthDate([FromQuery] Guid personId, [FromBody] DateOnly newBirthDate)
        {
            //if (newBirthDate == default)
            //{
            //    return BadRequest("BirthDate cannot be the default value.");
            //}

            //if (newBirthDate > DateTime.FromDateTime(DateTime.Now))
            //{
            //    return BadRequest("BirthDate cannot be in the future.");
            //}

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdateBirthDate(userId, personId, newBirthDate);
                return Ok("BirthDate updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePersonalIdNumber")]
        public async Task<IActionResult> UpdateIdNumber([FromQuery] Guid personId, [FromBody] string newPersonalIdNumber)
        {
            if (string.IsNullOrWhiteSpace(newPersonalIdNumber))
            {
                return BadRequest("ID number cannot be blank or whitespace.");
            }

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdateIdNumber(userId, personId, newPersonalIdNumber);
                return Ok("ID number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromQuery] Guid personId, [FromBody] string newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                return BadRequest("Phone number cannot be blank or whitespace.");
            }

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdatePhoneNumber(userId, personId, newPhoneNumber);
                return Ok("Phone number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromQuery] Guid personId, [FromBody] string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                return BadRequest("Phone number cannot be blank or whitespace.");
            }

            try
            {
                //var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdatePhoneNumber(userId, personId, newEmail);
                return Ok("Email updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePhoto")]
        public async Task<IActionResult> UpdatePhoto([FromQuery] Guid personId, [FromBody] string newFilePath)
        {
            if (string.IsNullOrWhiteSpace(newFilePath))
            {
                return BadRequest("Phone number cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _personService.UpdatePhoneNumber(userId, personId, newFilePath); ////check
                return Ok("Photo updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromQuery] Guid personId, [FromBody] string newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity))
            {
                return BadRequest("City cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
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
                return BadRequest("City cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
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
                return BadRequest("City cannot be blank or whitespace.");
            }

            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
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
                var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (userRole != "User")
                {
                    return Unauthorized("You are not authorized to perform this action.");
                }
                await _placeOfResidenceService.UpdateCity(userId, personId, newAppartmentNumber);
                return Ok("Apartment number updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //There must be a different endpoint to update EACH of the Models properties, e.g.Name, ID number, phone number, city(cannot be updated to a blank field or whitespace)



    }
}