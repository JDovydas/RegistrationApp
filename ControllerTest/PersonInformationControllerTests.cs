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
        public async Task UpdateGender_ShouldReturnOkResult_WhenGenderIsUpdatedSuccessfully()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var newGender = "Male";

            // Setup mock to simulate successful update
            // UpdateGenderAsync is called with any userId, specified personId, and newGender,
            // Return completed Task
            _personServiceMock.Setup(s => s.UpdateGenderAsync(It.IsAny<Guid>(), personId, newGender))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateGender(personId, newGender);

            // Assert
            // Check if result is of type OkObjectResult indicating successful operation.
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verify that message in OkObjectResult is "Gender updated successfully."
            Assert.Equal("Gender updated successfully.", okResult.Value);
        }
    }
}
