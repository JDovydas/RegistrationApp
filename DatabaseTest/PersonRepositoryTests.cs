using Microsoft.EntityFrameworkCore;
using RegistrationApp.Database.Repositories;
using RegistrationApp.Database;
using RegistrationApp.Shared.Models;

namespace DatabaseTest
{
    public class PersonRepositoryTests
    {
        // In-memory database context
        private readonly RegistrationAppContext _context;

        // Repository to be tested
        private readonly PersonRepository _personRepository;

        // Constructor to set up the in-memory database and repository for testing
        public PersonRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RegistrationAppContext>()
                .UseInMemoryDatabase(databaseName: "RegistrationAppTestDb") //Use an in-memory database for testing
                .Options;

            _context = new RegistrationAppContext(options); // Initialize the database context with the in-memory options
            _personRepository = new PersonRepository(_context); // Initialize the repository with the context

            // Seed the database with a test person
            _context.People.Add(new Person
            {
                Id = Guid.NewGuid(),
                Name = "Jonas",
                LastName = "Pirmasis",
                Gender = "Vyras",
                BirthDate = new DateOnly(1990, 1, 1),
                PersonalId = "1234567890",
                PhoneNumber = "+37060000000",
                Email = "jonas.pirmasis@gmail.com",
                FilePath = "path/to/photo"
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPersonByIdAsync_ReturnsPerson()
        {
            // Arrange
            var person = await _context.People.FirstOrDefaultAsync();

            // Act
            var result = await _personRepository.GetPersonByIdAsync(person.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person.Name, result.Name);
        }

        [Fact]
        public async Task AddPersonAsync_AddsPersonSuccessfully()
        {
            // Arrange
            var newPerson = new Person
            {
                Id = Guid.NewGuid(),
                Name = "Janina",
                LastName = "Pirmoji",
                Gender = "Moteris",
                BirthDate = new DateOnly(1992, 2, 2),
                PersonalId = "0987654321",
                PhoneNumber = "+37060000001",
                Email = "Janina.Pirmoji@gmail.com",
                FilePath = "path/to/photo2"
            };

            // Act
            await _personRepository.AddPersonAsync(newPerson);
            var savedPerson = await _context.People.FirstOrDefaultAsync(p => p.Name == "Janina");

            // Assert
            Assert.NotNull(savedPerson);
            Assert.Equal("Janina", savedPerson.Name);
        }

        [Fact]
        public async Task UpdatePersonAsync_UpdatesPersonSuccessfully()
        {
            // Arrange
            var person = await _context.People.FirstOrDefaultAsync();
            person.LastName = "Antrasis";

            // Act
            await _personRepository.UpdatePersonAsync(person);
            var updatedPerson = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);

            Assert.NotNull(updatedPerson);
            Assert.Equal("Antrasis", updatedPerson.LastName);
        }

        [Fact]
        public async Task DeletePersonAsync_DeletesPersonSuccessfully()
        {
            // Arrange
            var person = await _context.People.FirstOrDefaultAsync();

            // Act
            await _personRepository.DeletePersonAsync(person);
            var deletedPerson = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);

            // Assert
            Assert.Null(deletedPerson);
        }
    }
}
