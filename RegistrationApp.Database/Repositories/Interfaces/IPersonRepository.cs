using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> AddPerson(Person person);
        Task<Person> GetPersonById(Guid personId);
        Task<Person> UpdatePerson(Person person);
    }
}
