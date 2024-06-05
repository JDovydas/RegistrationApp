using RegistrationApp.Shared.Models;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(string username, string password);
        Task<User> Login(string username, string password);
        Task<User> SignUp(string username, string password);
        Task DeleteUserById(Guid userId);
    }
}
