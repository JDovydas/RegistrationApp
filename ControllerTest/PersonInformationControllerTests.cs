using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Controllers;
using RegistrationApp.Shared.DTOs;
using System;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControllerTest
{
    public class PersonInformationControllerTests
    {
        // Mock objects for interfaces
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Mock<IPlaceOfResidenceService> _placeOfResidenceServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        // Instance of the PersonInformationController class which will be tested
        private readonly PersonInformationController _controller;

        // Constructor to set up mocks and the controller instance for the tests
        public PersonInformationControllerTests()
        {
            // Initialize mocks for the services that the controller depends on
            _personServiceMock = new Mock<IPersonService>();
            _placeOfResidenceServiceMock = new Mock<IPlaceOfResidenceService>();
            _userServiceMock = new Mock<IUserService>();

            // Create an instance of the controller, passing the mocked service objects
            _controller = new PersonInformationController(_personServiceMock.Object, _placeOfResidenceServiceMock.Object, _userServiceMock.Object);

            // Create a new HttpContext and set up a mock user identity
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                // Adding a NameIdentifier claim to simulate a logged-in user
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                // Adding a Role claim to simulate the user's role
                new Claim(ClaimTypes.Role, "User")
            }));

            // Set the controller's context to use the created HttpContext with the mock user
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task AddPersonInformation_ShouldReturnOkResult_WhenPersonInfoIsAddedSuccessfully()
        {
            // Arrange
            var personDto = new PersonDto
            {
                Name = "Jonas",
                LastName = "Pirmasis",
                BirthDate = "1990-01-01"
            };

            var placeOfResidenceDto = new PlaceOfResidenceDto
            {
                City = "Vilnius",
                Street = "Gedimino pr.",
                HouseNumber = 1
            };

            // The birth date value that will be used in the mock setup.
            DateOnly birthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"));

            // Setting up the mock for the ValitateBirthDate method in the IPersonService interface
            // It is configured to return true when called with any string as an argument and to output the birthDate variable.
            _personServiceMock.Setup(s => s.ValitateBirthDate(It.IsAny<string>(), out birthDate))
                              // Callback method allows setting the out parameter when the ValitateBirthDate method is called.
                              .Callback((string input, out DateOnly date) =>
                              {
                                  // The out parameter is set to the specified birth date.
                                  date = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"));
                              })
                              // The Returns method specifies the return value of the mocked method.
                              .Returns(true);

            // Setting up the mock for the AddPersonInformationAsync method in the IPersonService interface.
            // It is configured to return a completed Task when called with any Guid, the specified personDto, placeOfResidenceDto, null, and the birthDate.
            _personServiceMock.Setup(s => s.AddPersonInformationAsync(It.IsAny<Guid>(), personDto, placeOfResidenceDto, null, birthDate))
                               // The Returns method specifies the return value of the mocked method.
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddPersonInformation(personDto, placeOfResidenceDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Person information added successfully.", okResult.Value);
        }

        [Fact]
        public async Task AddPersonInformation_ShouldReturnBadRequest_WhenBirthDateIsInvalid()
        {
            // Arrange
            var personDto = new PersonDto
            {
                Name = "Jonas",
                LastName = "Pirmasis",
                BirthDate = "invalid-date"
            };

            var placeOfResidenceDto = new PlaceOfResidenceDto
            {
                City = "Vilnius",
                Street = "Gedimino pr.",
                HouseNumber = 1
            };

            // Declare a variable to hold the out parameter value for birth date validation
            DateOnly birthDate;

            // Set up the mock for the ValitateBirthDate method in the IPersonService interface.
            // This is configured to return false when called with any string as an argument.
            _personServiceMock.Setup(s => s.ValitateBirthDate(It.IsAny<string>(), out birthDate))
                              .Returns(false);

            // Act
            var result = await _controller.AddPersonInformation(personDto, placeOfResidenceDto);

            // Assert
            // Check if the result is of type BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            // Verify that the value of the bad request result matches the expected error message.
            Assert.Equal("Invalid date format for BirthDate. Please use YYYY-MM-DD.", badRequestResult.Value);
        }


        [Fact]
        public async Task UpdateName_ShouldReturnOkResult_WhenNameIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newName = "Petras";

            // Setup the mock to simulate a successful update
            // UpdateNameAsync is called with any userId, the specified personId, and the newName,
            // it will return a completed Task, indicating that the update was successful.
            _personServiceMock.Setup(s => s.UpdateNameAsync(It.IsAny<Guid>(), personId, newName))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateName(personId, newName);

            // Assert
            // Check if the result is of type OkObjectResult, indicating a successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verify that the message in the OkObjectResult is "Name updated successfully."
            Assert.Equal("Name updated successfully.", okResult.Value);
        }


        // Example for UpdateLastName
        [Fact]
        public async Task UpdateLastName_ShouldReturnOkResult_WhenLastNameIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newLastName = "NewLastName";

            // Setup the mock to simulate a successful update
            // This means when UpdateLastNameAsync is called with any userId, the specified personId and the newLastName,
            // it will return a completed Task, indicating that the update was successful.
            _personServiceMock.Setup(s => s.UpdateLastNameAsync(It.IsAny<Guid>(), personId, newLastName))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateLastName(personId, newLastName);

            // Assert
            // Check if the result is of type OkObjectResult, indicating a successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verify that the message in the OkObjectResult is "Last name updated successfully."
            Assert.Equal("Last name updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateGender_ShouldReturnOkResult_WhenGenderIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newGender = "Male";

            // Setup the mock to simulate a successful update
            // This means when UpdateGenderAsync is called with any userId, the specified personId, and the newGender,
            // it will return a completed Task, indicating that the update was successful.
            _personServiceMock.Setup(s => s.UpdateGenderAsync(It.IsAny<Guid>(), personId, newGender))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateGender(personId, newGender);

            // Assert
            // Check if the result is of type OkObjectResult, indicating a successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verify that the message in the OkObjectResult is "Gender updated successfully."
            Assert.Equal("Gender updated successfully.", okResult.Value);
        }
    }
}
