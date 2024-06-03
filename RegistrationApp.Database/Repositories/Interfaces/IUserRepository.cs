using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddNewUser(User user);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(Guid userId);
        Task<User> DeleteUser(User user);
    }
}
