using RegistrationApp.Shared.Models;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string username, string password);
        Task<User> LoginAsync(string username, string password);
        Task<User> SignUpAsync(string username, string password);
        Task DeleteUserByIdAsync(Guid userId);
    }
}
