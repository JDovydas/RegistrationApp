using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using System.Security.Cryptography;

namespace RegistrationApp.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> SignUpAsync(string username, string password)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Such user already exists");
            }
            var newUser = await CreateUserAsync(username, password);
            return await _userRepository.AddNewUserAsync(newUser);
        }

        public async Task<User> CreateUserAsync(string username, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };
            return newUser;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist");
            }

            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
            {
                throw new InvalidOperationException("Incorrect password");
            }
            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        public async Task DeleteUserByIdAsync(Guid userId)
        {
            var userToDelete = await _userRepository.GetUserByIdAsync(userId);
            if (userToDelete == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }
            await _userRepository.DeleteUserAsync(userToDelete);
        }
    }
}
