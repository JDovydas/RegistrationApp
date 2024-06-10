using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPlaceOfResidenceRepository
    {
        Task AddPlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);
        Task UpdatePlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);
        Task<PlaceOfResidence> GetPlaceOfResidenceByPersonIdAsync(Guid personId);
    }
}
