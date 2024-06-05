using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddNewUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);
        Task DeleteUserAsync(User user);
    }
}
