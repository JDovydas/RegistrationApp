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
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        // Instance of the UserController class which will be tested
        private readonly UserController _controller;

        // Constructor to set up mocks and the controller instance for the tests
        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _jwtServiceMock = new Mock<IJwtService>();
            _controller = new UserController(_userServiceMock.Object, _jwtServiceMock.Object);
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

            // Mocking the SignUpAsync method to return a new User object
            _userServiceMock.Setup(service => service.SignUpAsync(userDto.Username, userDto.Password)).ReturnsAsync(new User { Username = userDto.Username });

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

            // Mocking the DeleteUserByIdAsync method to complete successfully
            _userServiceMock.Setup(service => service.DeleteUserByIdAsync(userId)).Returns(Task.CompletedTask);

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

            // Mocking the DeleteUserByIdAsync method to throw an InvalidOperationException
            _userServiceMock.Setup(service => service.DeleteUserByIdAsync(userId)).ThrowsAsync(new InvalidOperationException("User not found."));

            // Act
            var result = await _controller.DeleteUserAsync(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found.", badRequestResult.Value);
        }
    }
}
