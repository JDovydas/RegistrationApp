using Moq;
using RegistrationApp.BusinessLogic.Services;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BusinessLogicTest
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task SignUpAsync_AddsUserSuccessfully()
        {
            // Arrange
            var username = "newuser";
            var password = "Password123!";
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = username
            };

            //Mocked to return the newUser - simulates the successful addition of the user to the system.
            _userRepositoryMock.Setup(repo => repo.AddNewUserAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            // Act
            var user = await _userService.SignUpAsync(username, password);

            // Assert - is called exactly once with any User object
            _userRepositoryMock.Verify(repo => repo.AddNewUserAsync(It.IsAny<User>()), Times.Once);
            Assert.Equal(username, user.Username);
        }

        [Fact]
        public async Task SignUpAsync_ThrowsException_WhenUserAlreadyExists()
        {
            // Arrange
            var username = "existinguser";
            var password = "Password123!";
            var existingUser = new User
            {
                Username = username
            };

            // Mocking the GetUserByIdAsync - it simulates that a user with the given username already exists in the system.
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.SignUpAsync(username, password));
            Assert.Equal("Such user already exists", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ReturnsUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var username = "existinguser";
            var password = "Password123!";
            var user = new User();
            _userService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = username;
            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Mocking GetUserByUsernameAsync to return the user object, simulating that the user exists in the system.
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task LoginAsync_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";
            var password = "Password123!";

            //Mocking GetUserByUsernameAsync to return null, simulating that the user does not exist in the system.
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync((User)null);

            // Act & Assert
            ////Calling LoginAsync method with the username and password, and use Assert.ThrowsAsync<InvalidOperationException> to check that an InvalidOperationException is thrown.
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.LoginAsync(username, password));
            Assert.Equal("User does not exist", exception.Message);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_DeletesUserSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "testuser"
            };

            // Setup sequence to return the user first and then null after deletion
            _userRepositoryMock.SetupSequence(repo => repo.GetUserByIdAsync(userId))
                               .ReturnsAsync(user)  // First call returns the user
                               .ReturnsAsync((User)null);  // Subsequent calls return null

            // Act
            await _userService.DeleteUserByIdAsync(userId);

            // Assert that subsequent calls to GetUserByIdAsync return null
            var deletedUser = await _userRepositoryMock.Object.GetUserByIdAsync(userId);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            //Mocking GetUserByUsernameAsync to return null, simulating that the user does not exist in the system.
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.DeleteUserByIdAsync(userId));
            Assert.Equal("User does not exist.", exception.Message);
        }
    }
}
