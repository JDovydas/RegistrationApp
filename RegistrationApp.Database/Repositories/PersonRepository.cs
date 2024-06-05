﻿using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;
using Serilog;
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

        public async Task<Person> GetPersonByIdAsync(Guid personId)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);
            return person;
        }

        public async Task AddPersonAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            await _context.People.FindAsync(person.Id);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(Person person)
        {
            var personToDelete = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);
            try
            {
                _context.People.Remove(personToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"[{nameof(DeletePersonAsync)}]: {ex.Message}");
                throw;
            }
            Log.Information($"[{nameof(DeletePersonAsync)}]: Successfully removed User with ID: {person.Id}");

        }
    }
}
