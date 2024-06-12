using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using Serilog;
using System;

namespace RegistrationApp.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Database context for accessing database
        private readonly RegistrationAppContext _context;

        // Constructor to inject database context
        public UserRepository(RegistrationAppContext appContext)
        {
            _context = appContext;
        }

        public async Task<User> AddNewUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userToDelete == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

        }
    }
}
