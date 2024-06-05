using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using Serilog;

namespace RegistrationApp.Database.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly RegistrationAppContext _context;

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
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByIdAsync(Guid userId) //Check if People need to be included
        {
            return await _context.Users.Include(p => p.People).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task DeleteUserAsync(User user)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            try
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"[{nameof(DeleteUserAsync)}]: {ex.Message}");
                throw;
            }
            Log.Information($"[{nameof(DeleteUserAsync)}]: Successfully removed User with ID: {user.Id}");
        }

    }
}
