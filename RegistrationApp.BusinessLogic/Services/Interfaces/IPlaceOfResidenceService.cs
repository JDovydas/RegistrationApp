using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPlaceOfResidenceService
    {
        Task<PlaceOfResidenceDto> UpdateCity(Guid userId, Guid personId, string newCity);
        Task<PlaceOfResidenceDto> UpdateStreet(Guid userId, Guid personId, string newStreet);
        Task<PlaceOfResidenceDto> UpdateHouseNumber(Guid userId, Guid personId, int newHouseNumber);
        Task<PlaceOfResidenceDto> UpdateAppartmentNumber(Guid userId, Guid personId, int newHouseNumber);

    }
}
