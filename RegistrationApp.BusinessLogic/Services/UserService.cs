using Microsoft.EntityFrameworkCore;
using RegistrationApp.BusinessLogic.Helpers;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Database.Repositories;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using System.Security.Cryptography;

namespace RegistrationApp.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        // Constructor to inject IUserRepository dependency
        public UserService(IUserRepository userRepository, IPersonRepository personRepository)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
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
            // Create password hash and salt
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
            // Using HMACSHA512 cryptographic algorithm to generate hash and salt
            using var hmac = new HMACSHA512();

            // Generate random key to be used as salt
            passwordSalt = hmac.Key;

            // Compute hash of password byte array
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Initialize new instance of HMACSHA512 cryptographic algorithm with provided salt (key)
            using var hmac = new HMACSHA512(passwordSalt);

            // Convert password string to byte array using UTF-8 encoding
            //generates hash by combining password bytes and salt
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            //Compare computed hash with provided hash
            // SequenceEqual method checks if both byte arrays contain same elements in the same order
            return computedHash.SequenceEqual(passwordHash);
        }

        public async Task DeleteUserByIdAsync(Guid userId)
        {
            var userToDelete = await _userRepository.GetUserByIdAsync(userId);
            if (userToDelete == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            // Retrieve all people associated with the user
            var peopleToDelete = await _personRepository.GetPeopleByUserIdAsync(userId);

            // Delete associated people photos from System
            foreach (var person in peopleToDelete)
            {
                if (!string.IsNullOrEmpty(person.FilePath))
                {
                    ProfilePhotoHelpers.DeleteProfilePhoto(person.FilePath);
                }
            }
            await _userRepository.DeleteUserAsync(userToDelete);
        }
    }
}
