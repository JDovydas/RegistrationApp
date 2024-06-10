using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        // Database context for accessing database
        private readonly RegistrationAppContext _context;

        // Constructor to inject database context
        public PersonRepository(RegistrationAppContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonByIdAsync(Guid personId)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            return person;
        }

        public async Task AddPersonAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            var personToUpdate = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);
            if (personToUpdate == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            _context.Update(personToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(Person person)
        {
            var personToDelete = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);
            if (personToDelete == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            _context.People.Remove(personToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
