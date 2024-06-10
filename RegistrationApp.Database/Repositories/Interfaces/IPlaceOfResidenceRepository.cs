using RegistrationApp.Shared.Models;

namespace RegistrationApp.Database.Repositories.Interfaces
{
    public interface IPlaceOfResidenceRepository
    {
        //Task<PlaceOfResidence> AddPlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);
        Task AddPlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);
        //Task<PlaceOfResidence> UpdatePlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);
        Task UpdatePlaceOfResidenceAsync(PlaceOfResidence placeOfResidence);

        Task<PlaceOfResidence> GetPlaceOfResidenceByPersonIdAsync(Guid personId);
    }
}
