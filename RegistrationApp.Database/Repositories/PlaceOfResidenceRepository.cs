using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories
{
    public class PlaceOfResidenceRepository : IPlaceOfResidenceRepository
    {
        // Database context for accessing database
        private readonly RegistrationAppContext _context;

        // Constructor to inject database context
        public PlaceOfResidenceRepository(RegistrationAppContext context)
        {
            _context = context;
        }

        public async Task AddPlaceOfResidenceAsync(PlaceOfResidence placeOfResidence)
        {
            await _context.PlacesOfResidence.AddAsync(placeOfResidence);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlaceOfResidenceAsync(PlaceOfResidence placeOfResidence)
        {
            var existingPlaceOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.Id == placeOfResidence.Id);
            if (existingPlaceOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            _context.Update(placeOfResidence);
            await _context.SaveChangesAsync();
        }

        public async Task<PlaceOfResidence> GetPlaceOfResidenceByPersonIdAsync(Guid personId)
        {
            var placeOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.PersonId == personId);
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            return placeOfResidence;
        }
    }
}
