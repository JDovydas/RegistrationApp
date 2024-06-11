using Microsoft.AspNetCore.Mvc;
using Moq;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Controllers;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;

namespace ControllerTest
{
    public class UserControllerTests
    {
        // Mock objects for IUserService and IJwtService interfaces
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IJwtService> _mockJwtServiceMock;
        // Instance of UserController class which will be tested
        private readonly UserController _controller;

        // Constructor to set up mocks and controller instance for tests
        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockJwtServiceMock = new Mock<IJwtService>();
            _controller = new UserController(_mockUserService.Object, _mockJwtServiceMock.Object);
        }

        [Fact]
        public async Task SignUp_ReturnsOkResult()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "newuser",
                Password = "Password123!"
            };

            // Mocking the SignUpAsync method to return new User object
            _mockUserService.Setup(service => service.SignUpAsync(userDto.Username, userDto.Password)).ReturnsAsync(new User { Username = userDto.Username });

            // Act
            var result = await _controller.SignUp(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Mocking DeleteUserByIdAsync method to complete successfully
            _mockUserService.Setup(service => service.DeleteUserByIdAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUserAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User deleted successfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ReturnsBadRequest_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Mocking DeleteUserByIdAsync method to throw InvalidOperationException
            _mockUserService.Setup(service => service.DeleteUserByIdAsync(userId)).ThrowsAsync(new InvalidOperationException("User not found."));

            // Act
            var result = await _controller.DeleteUserAsync(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found.", badRequestResult.Value);
        }
    }
}
