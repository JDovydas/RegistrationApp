using Moq;
using RegistrationApp.BusinessLogic.Services;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;

namespace BusinessLogicTest
{
    public class PersonServiceTests
    {
        //Mocking
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPlaceOfResidenceRepository> _mockPlaceOfResidenceRepository;

        //Instance
        private readonly PersonService _personService;

        //Constructor for PersonServiceTests class - sets up mocks and service instance for tests
        public PersonServiceTests()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPlaceOfResidenceRepository = new Mock<IPlaceOfResidenceRepository>();
            _personService = new PersonService(_mockPersonRepository.Object, _mockUserRepository.Object, _mockPlaceOfResidenceRepository.Object); //Pass  mock objects of  interfaces as a dependency.
        }

        [Fact]
        public async Task AddPersonInformationAsync_ShouldAddPersonSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var personDto = new PersonDto
            {
                Name = "Jonas",
                LastName = "Pirmasis",
                Gender = "Male",
                BirthDate = "1980-01-01",
                PersonalId = "1234567890",
                PhoneNumber = "+37061111111",
                Email = "jonas.pirmasis@gmail.com"
            };
            var placeOfResidenceDto = new PlaceOfResidenceDto
            {
                City = "Vilnius",
                Street = "Gedimino pr",
                HouseNumber = 1,
                AppartmentNumber = 1
            };
            var user = new User
            {
                Id = userId
            };

            // Mocking GetUserByIdAsync to return created user
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            // Call AddPersonInformationAsync method to add person information
            await _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, null, DateOnly.Parse("1980-01-01"));

            // Assert
            // Verifying person repository was called exactly once with any Person object.
            _mockPersonRepository.Verify(repo => repo.AddPersonAsync(It.IsAny<Person>()), Times.Once);
            // Verifying place of residence repository was called exactly once with any PlaceOfResidence object
            _mockPlaceOfResidenceRepository.Verify(repo => repo.AddPlaceOfResidenceAsync(It.IsAny<PlaceOfResidence>()), Times.Once);
        }

        [Fact]
        public async Task AddPersonInformationAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var personDto = new PersonDto();
            var placeOfResidenceDto = new PlaceOfResidenceDto();
            var birthDate = DateOnly.Parse("1980-01-01");

            // Mocking GetUserByIdAsync to return null
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            // Assert throws InvalidOperationException
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, null, birthDate));
        }
    }
}
