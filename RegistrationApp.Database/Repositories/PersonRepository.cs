using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using System;

namespace RegistrationApp.Database.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly RegistrationAppContext _context;

        public PersonRepository(RegistrationAppContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonById(Guid personId)
        {
            return await _context.People.FirstOrDefaultAsync(p => p.Id == personId);
            //return await _context.People.Include(p => p.PlaceOfResidence).FirstOrDefaultAsync(p => p.Id == personId); - could that be used?
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var existingPerson = await _context.People.FindAsync(person.Id);
            if (existingPerson == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            existingPerson.Name = person.Name;
            existingPerson.LastName = person.LastName;
            existingPerson.Gender = person.Gender;
            existingPerson.BirthDate = person.BirthDate;
            existingPerson.PersonalId = person.PersonalId;
            existingPerson.PhoneNumber = person.PhoneNumber;
            existingPerson.Email = person.Email;
            existingPerson.FilePath = person.FilePath;
            existingPerson.PlaceOfResidenceId = person.PlaceOfResidenceId;
            existingPerson.UserId = person.UserId;

            await _context.SaveChangesAsync();
            return existingPerson;

        }
    }
}
