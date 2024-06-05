using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Database.Repositories
{
    public class PlaceOfResidenceRepository : IPlaceOfResidenceRepository
    {
        private readonly RegistrationAppContext _context;

        public PlaceOfResidenceRepository(RegistrationAppContext context)
        {
            _context = context;
        }

        public async Task<PlaceOfResidence> AddPlaceOfResidence(PlaceOfResidence placeOfResidence)
        {
            await _context.PlacesOfResidence.AddAsync(placeOfResidence);
            await _context.SaveChangesAsync();
            return placeOfResidence;
        }

        public async Task<PlaceOfResidence> UpdatePlaceOfResidence(PlaceOfResidence placeOfResidence)
        {
            var existingplaceOfResidence = await _context.PlacesOfResidence.FindAsync(placeOfResidence.Id);
            if (existingplaceOfResidence == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            existingplaceOfResidence.City = placeOfResidence.City;
            existingplaceOfResidence.Street = placeOfResidence.Street;
            existingplaceOfResidence.HouseNumber = placeOfResidence.HouseNumber;
            existingplaceOfResidence.AppartmentNumber = placeOfResidence.AppartmentNumber;
            existingplaceOfResidence.Person = placeOfResidence.Person;// This one to be passed from earlier?


            await _context.SaveChangesAsync();
            return existingplaceOfResidence;
        }

        public async Task<PlaceOfResidence> GetPlaceOfResidenceByPersonId(Guid personId)
        {
            var placeOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.PersonId == personId);
            return placeOfResidence;
        }


    }
}
