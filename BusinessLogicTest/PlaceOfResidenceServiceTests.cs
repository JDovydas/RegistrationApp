using Moq;
using RegistrationApp.BusinessLogic.Services;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.Models;

namespace BusinessLogicTest
{
    public class PlaceOfResidenceServiceTests
    {
        //mock objects for interfaces
        private readonly Mock<IPlaceOfResidenceRepository> _placeOfResidenceRepositoryMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        //instance of PlaceOfResidenceService class which will be tested
        private readonly PlaceOfResidenceService _placeOfResidenceService;

        //constructor for PlaceOfResidenceService class - sets up mocks and the service instance for tests
        public PlaceOfResidenceServiceTests()
        {
            _placeOfResidenceRepositoryMock = new Mock<IPlaceOfResidenceRepository>();
            _personRepositoryMock = new Mock<IPersonRepository>();
            _placeOfResidenceService = new PlaceOfResidenceService(_placeOfResidenceRepositoryMock.Object, _personRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateCityAsync_ShouldUpdateCitySuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                UserId = userId,
                PlaceOfResidence = new PlaceOfResidence
                {
                    Id = Guid.NewGuid(),
                    City = "OldCity"
                }
            };
            // Mocking the GetUserByIdAsync to return the created person
            _personRepositoryMock.Setup(repo => repo.GetPersonByIdAsync(personId)).ReturnsAsync(person);

            // Act
            await _placeOfResidenceService.UpdateCityAsync(userId, personId, "Kaunas");

            // Assert
            Assert.Equal("Kaunas", person.PlaceOfResidence.City);

        }
    }
}
