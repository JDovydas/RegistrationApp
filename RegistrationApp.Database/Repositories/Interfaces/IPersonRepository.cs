using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonByIdAsync(Guid personId);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(Person person);
    }
}
