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

        public async Task<User> AddNewUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _context.Users.Include(p => p.People).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> DeleteUser(User user)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            //should I include e.g. public ICollection<Person> People { get; set; } deletion -- Include???

            try
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"[{nameof(DeleteUser)}]: {ex.Message}");
                throw;
            }
            Log.Information($"[{nameof(DeleteUser)}]: Successfully removed User with ID: {user.Id}");
            return userToDelete;
        }

    }
}
