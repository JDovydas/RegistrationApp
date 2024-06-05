using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPlaceOfResidenceService
    {
        Task<PlaceOfResidenceDto> UpdateCityAsync(Guid userId, Guid personId, string newCity);
        Task<PlaceOfResidenceDto> UpdateStreetAsync(Guid userId, Guid personId, string newStreet);
        Task<PlaceOfResidenceDto> UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber);
        Task<PlaceOfResidenceDto> UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newHouseNumber);
    }
}
