using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories;
using RegistrationApp.Database;
using RegistrationApp.Shared.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DatabaseTest
{
    public class PlaceOfResidenceRepositoryTests
    {
        // In-memory database context
        private readonly RegistrationAppContext _context;

        // Repository to be tested
        private readonly PlaceOfResidenceRepository _placeOfResidenceRepository;

        // Constructor to set up the in-memory database and repository for testing
        public PlaceOfResidenceRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RegistrationAppContext>()
                .UseInMemoryDatabase(databaseName: "RegistrationAppTestDb") // Use an in-memory database for testing
                .Options;

            _context = new RegistrationAppContext(options); // Initialize the database context with the in-memory options
            _placeOfResidenceRepository = new PlaceOfResidenceRepository(_context); // Initialize the repository with the context

            // Seed the database with a test place of residence
            _context.PlacesOfResidence.Add(new PlaceOfResidence
            {
                Id = Guid.NewGuid(),
                City = "Vilnius",
                Street = "Gedimino pr.",
                HouseNumber = 1,
                AppartmentNumber = 1
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPlaceOfResidenceByPersonIdAsync_ReturnsPlaceOfResidence()
        {
            // Arrange
            var placeOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync();
            var personId = Guid.NewGuid();
            placeOfResidence.PersonId = personId;
            await _context.SaveChangesAsync();

            // Act
            var result = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);

            Assert.NotNull(result);
            Assert.Equal(placeOfResidence.City, result.City);
        }

        [Fact]
        public async Task AddPlaceOfResidenceAsync_AddsPlaceOfResidenceSuccessfully()
        {
            // Arrange
            var newPlaceOfResidence = new PlaceOfResidence
            {
                Id = Guid.NewGuid(),
                City = "Kaunas",
                Street = "Laisvės al.",
                HouseNumber = 2,
                AppartmentNumber = 3
            };

            // Act
            await _placeOfResidenceRepository.AddPlaceOfResidenceAsync(newPlaceOfResidence);
            var savedPlaceOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.City == "Kaunas");

            // Assert
            Assert.NotNull(savedPlaceOfResidence);
            Assert.Equal("Kaunas", savedPlaceOfResidence.City);
        }

        [Fact]
        public async Task UpdatePlaceOfResidenceAsync_UpdatesPlaceOfResidenceSuccessfully()
        {
            // Arrange
            var placeOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync();
            placeOfResidence.City = "Klaipėda";

            // Act
            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
            var updatedPlaceOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.Id == placeOfResidence.Id);

            // Assert
            Assert.NotNull(updatedPlaceOfResidence);
            Assert.Equal("Klaipėda", updatedPlaceOfResidence.City);
        }

        [Fact]
        public async Task DeletePlaceOfResidenceAsync_DeletesPlaceOfResidenceSuccessfully()
        {
            // Arrange
            var placeOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync();

            _context.PlacesOfResidence.Remove(placeOfResidence);
            await _context.SaveChangesAsync();

            var deletedPlaceOfResidence = await _context.PlacesOfResidence.FirstOrDefaultAsync(p => p.Id == placeOfResidence.Id);

            Assert.Null(deletedPlaceOfResidence);
        }
    }
}
