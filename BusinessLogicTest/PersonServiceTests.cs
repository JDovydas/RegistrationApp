using Moq;
using RegistrationApp.BusinessLogic.Services;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;

namespace BusinessLogicTest
{
    public class PersonServiceTests
    {
        //mock objects for the interfaces
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPlaceOfResidenceRepository> _placeOfResidenceRepositoryMock;

        //instance of the PersonService class which will be tested
        private readonly PersonService _personService;

        //constructor for the PersonServiceTests class - sets up mocks and the service instance for the tests
        public PersonServiceTests()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _placeOfResidenceRepositoryMock = new Mock<IPlaceOfResidenceRepository>();
            _personService = new PersonService(_personRepositoryMock.Object, _userRepositoryMock.Object, _placeOfResidenceRepositoryMock.Object); //Passe the mock object of the interface as a dependency.
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

            // Mocking the GetUserByIdAsync to return the created user
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            // Call the AddPersonInformationAsync method to add person information
            await _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, null, DateOnly.Parse("1980-01-01"));

            // Assert
            // Verifying that AddPersonAsync method of the person repository was called exactly once with any Person object.
            // This ensures that Person information was added to the repository.
            _personRepositoryMock.Verify(repo => repo.AddPersonAsync(It.IsAny<Person>()), Times.Once);
            // Verifying that AddPlaceOfResidenceAsync method of the place of residence repository was called exactly once with any PlaceOfResidence object
            // This ensures that PlaceOfResidence information was added to the repository.
            _placeOfResidenceRepositoryMock.Verify(repo => repo.AddPlaceOfResidenceAsync(It.IsAny<PlaceOfResidence>()), Times.Once);
        }

        [Fact]
        public async Task AddPersonInformationAsync_ShouldThrowException_WhenUserNotFound()
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
                Street = "Gedimino pr.",
                HouseNumber = 1,
                AppartmentNumber = 1
            };
            var birthDate = DateOnly.Parse("1980-01-01");

            // Set up the user repository mock to return null when GetUserByIdAsync is called with the generated user ID
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            // Assert that the AddPersonInformationAsync method throws an InvalidOperationException
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _personService.AddPersonInformationAsync(userId, personDto, placeOfResidenceDto, null, birthDate));
        }
    }
}
