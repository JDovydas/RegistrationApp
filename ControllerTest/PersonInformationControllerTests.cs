using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Controllers;
using RegistrationApp.Shared.DTOs;
using System.Security.Claims;

namespace ControllerTest
{
    public class PersonInformationControllerTests
    {
        // Mock objects for interfaces
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Mock<IPlaceOfResidenceService> _placeOfResidenceServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        // Instance of PersonInformationController class which will be tested
        private readonly PersonInformationController _controller;

        // Constructor to set up mocks and controller instance for tests
        public PersonInformationControllerTests()
        {
            // Initialize mocks for services
            _personServiceMock = new Mock<IPersonService>();
            _placeOfResidenceServiceMock = new Mock<IPlaceOfResidenceService>();
            _userServiceMock = new Mock<IUserService>();

            // Create instance of controller, passing mocked service objects
            _controller = new PersonInformationController(_personServiceMock.Object, _placeOfResidenceServiceMock.Object, _userServiceMock.Object);

            // Create new HttpContext and set up mock user identity
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                // Adding NameIdentifier claim to simulate logged-in user
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                // Adding Role claim to simulate the user's role
                new Claim(ClaimTypes.Role, "User")
            }));

            // Set controller's context to use created HttpContext with mock user
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

            // Birth date will to be used in mock setup.
            DateOnly birthDate = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"));

            // returns true when called with any string as argument and to output birthDate variable.
            _personServiceMock.Setup(s => s.ValitateBirthDate(It.IsAny<string>(), out birthDate))
                              // Callback method allows setting out parameter when ValitateBirthDate method is called.
                              .Callback((string input, out DateOnly date) =>
                              {
                                  // Out parameter is set to specified birth date.
                                  date = DateOnly.FromDateTime(DateTime.Parse("1990-01-01"));
                              })
                              .Returns(true);

            // mock - return completed Task when called with any Guid, specified personDto, placeOfResidenceDto, null and birthDate.
            _personServiceMock.Setup(s => s.AddPersonInformationAsync(It.IsAny<Guid>(), personDto, placeOfResidenceDto, null, birthDate))
                               // Returns method specifies return value of mocked method.
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddPersonInformation(personDto, placeOfResidenceDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Person information added successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateName_ShouldReturnOkResult_WhenNameIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newName = "Petras";

            // Setup mock to simulate a successful update
            // UpdateNameAsync is called with any userId, specified personId, and newName,
            // Return completed Task, indicating that update was successful.
            _personServiceMock.Setup(s => s.UpdateNameAsync(It.IsAny<Guid>(), personId, newName))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateName(personId, newName);

            // Assert
            // Check if result is of type OkObjectResult, indicating successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verify that message in OkObjectResult is "Name updated successfully."
            Assert.Equal("Name updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateGender_ShouldReturnOkResult_WhenGenderIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newGender = "Male";

            // Setup mock to simulate successful update
            // UpdateGenderAsync is called with any userId, specified personId, and newGender,
            // Return completed Task, indicating that update was successful.
            _personServiceMock.Setup(s => s.UpdateGenderAsync(It.IsAny<Guid>(), personId, newGender))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateGender(personId, newGender);

            // Assert
            // Check if result is of type OkObjectResult, indicating successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verify that message in OkObjectResult is "Gender updated successfully."
            Assert.Equal("Gender updated successfully.", okResult.Value);
        }
    }
}
