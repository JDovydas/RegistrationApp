using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories;
using RegistrationApp.Database;
using RegistrationApp.Shared.Models;

namespace DatabaseTest
{
    public class UserRepositoryTests
    {
        private readonly RegistrationAppContext _context;
        private readonly UserRepository _userRepository;

        // Constructor to set up the in-memory database and repository for testing
        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RegistrationAppContext>()
                .UseInMemoryDatabase(databaseName: "RegistrationAppTestDb") // Use an in-memory database for testing
                .Options;

            _context = new RegistrationAppContext(options);// Initialize the database context with the in-memory options
            _userRepository = new UserRepository(_context);// Initialize the repository with the context

            // Seed the database with a test user
            _context.Users.Add(new User
            {
                Username = "testuser",
                Password = ConvertStringToByteArray("passwordHash"),
                PasswordSalt = ConvertStringToByteArray("salt"),
                Role = "User"
            });

            _context.SaveChanges();
        }

        // Helper method to convert a string to a byte array
        private byte[] ConvertStringToByteArray(string input)
        {
            return System.Text.Encoding.UTF8.GetBytes(input);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsUser()
        {
            // Act
            var result = await _userRepository.GetUserByUsernameAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task AddNewUserAsync_AddsUserSuccessfully()
        {
            // Arrange
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "newuser",
                Password = ConvertStringToByteArray("newpasswordHash"),
                PasswordSalt = ConvertStringToByteArray("newsalt"),
                Role = "User"
            };

            // Act
            await _userRepository.AddNewUserAsync(newUser);
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");

            // Assert
            Assert.NotNull(savedUser);
            Assert.Equal("newuser", savedUser.Username);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUserSuccessfully()
        {
            // Arrange
            var user = await _context.Users.FirstOrDefaultAsync();

            // Act
            await _userRepository.DeleteUserAsync(user);
            var deletedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            Assert.Null(deletedUser);
        }
    }
}
