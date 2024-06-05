using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task AddPerson(Person person);
        Task<Person> GetPersonById(Guid personId);
        Task UpdatePerson(Person person);
        Task DeletePerson(Person person);

    }
}
