using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.BusinessLogic.Helpers;

namespace RegistrationApp.BusinessLogic.Services
{
    public class PlaceOfResidenceService : IPlaceOfResidenceService
    {
        private readonly IPlaceOfResidenceRepository _placeOfResidenceRepository;
        private readonly IPersonRepository _personRepository;

        public PlaceOfResidenceService(IPlaceOfResidenceRepository placeOfResidenceRepository, IPersonRepository personRepository)
        {
            _placeOfResidenceRepository = placeOfResidenceRepository;
            _personRepository = personRepository;
        }

        public async Task UpdateCityAsync(Guid userId, Guid personId, string newCity)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.City = newCity;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
        }

        public async Task UpdateStreetAsync(Guid userId, Guid personId, string newStreet)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.Street = newStreet;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
        }

        public async Task UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.HouseNumber = newHouseNumber;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
        }

        public async Task UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newApparmentNumber)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.AppartmentNumber = newApparmentNumber;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
        }
    }
}
