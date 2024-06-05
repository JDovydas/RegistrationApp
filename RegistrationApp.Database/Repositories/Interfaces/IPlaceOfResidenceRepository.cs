using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPlaceOfResidenceRepository
    {
        Task<PlaceOfResidence> AddPlaceOfResidence(PlaceOfResidence placeOfResidence);
        Task<PlaceOfResidence> UpdatePlaceOfResidence(PlaceOfResidence placeOfResidence);

        Task<PlaceOfResidence> GetPlaceOfResidenceByPersonId(Guid personId);
    }
}
