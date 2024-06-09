using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPlaceOfResidenceService
    {
        Task UpdateCityAsync(Guid userId, Guid personId, string newCity);
        //Task<PlaceOfResidenceDto> UpdateCityAsync(Guid userId, Guid personId, string newCity);
        Task UpdateStreetAsync(Guid userId, Guid personId, string newStreet);
        //Task<PlaceOfResidenceDto> UpdateStreetAsync(Guid userId, Guid personId, string newStreet);
        Task UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber);
        //Task<PlaceOfResidenceDto> UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber);
        Task UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newHouseNumber);
        //Task<PlaceOfResidenceDto> UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newHouseNumber);
    }
}
