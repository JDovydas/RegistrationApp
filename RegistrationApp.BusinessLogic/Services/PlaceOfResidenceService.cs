using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistrationApp.Database.Repositories;

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
        //public async Task<PlaceOfResidenceDto> UpdateCityAsync(Guid userId, Guid personId, string newCity)
        {
            await EnsureUserOwnsPersonAsync(userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            var placeOfResidence = person.PlaceOfResidence;
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.City = newCity;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);

            //var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            //{
            //    City = placeOfResidence.City,
            //    Street = placeOfResidence.Street,
            //    HouseNumber = placeOfResidence.HouseNumber,
            //    AppartmentNumber = placeOfResidence.AppartmentNumber
            //};

            //return updatedPlaceOfResidenceDto;
        }

        public async Task UpdateStreetAsync(Guid userId, Guid personId, string newStreet)
        //public async Task<PlaceOfResidenceDto> UpdateStreetAsync(Guid userId, Guid personId, string newStreet)
        {
            await EnsureUserOwnsPersonAsync(userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            var placeOfResidence = person.PlaceOfResidence;
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.Street = newStreet;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);

            //var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            //{
            //    City = placeOfResidence.City,
            //    Street = placeOfResidence.Street,
            //    HouseNumber = placeOfResidence.HouseNumber,
            //    AppartmentNumber = placeOfResidence.AppartmentNumber
            //};

            //return updatedPlaceOfResidenceDto;
        }

        public async Task UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber)
        //public async Task<PlaceOfResidenceDto> UpdateHouseNumberAsync(Guid userId, Guid personId, int newHouseNumber)
        {
            await EnsureUserOwnsPersonAsync(userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            var placeOfResidence = person.PlaceOfResidence;
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.HouseNumber = newHouseNumber;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);

            //var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            //{
            //    City = placeOfResidence.City,
            //    Street = placeOfResidence.Street,
            //    HouseNumber = placeOfResidence.HouseNumber,
            //    AppartmentNumber = placeOfResidence.AppartmentNumber
            //};

            //return updatedPlaceOfResidenceDto;

        }

        public async Task UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newApparmentNumber)
        //public async Task<PlaceOfResidenceDto> UpdateAppartmentNumberAsync(Guid userId, Guid personId, int newApparmentNumber)
        {
            await EnsureUserOwnsPersonAsync(userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            var placeOfResidence = person.PlaceOfResidence;
            if (placeOfResidence == null)
            {
                throw new InvalidOperationException("Place of residence not found.");
            }

            placeOfResidence.AppartmentNumber = newApparmentNumber;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);

            //var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            //{
            //    City = placeOfResidence.City,
            //    Street = placeOfResidence.Street,
            //    HouseNumber = placeOfResidence.HouseNumber,
            //    AppartmentNumber = placeOfResidence.AppartmentNumber
            //};

            //return updatedPlaceOfResidenceDto;
        }

        public async Task EnsureUserOwnsPersonAsync(Guid userId, Guid personId) /// Duplicate method should it be added to Helpers? Is it OK that Task does not contain a  model in itself?
        {
            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            if (person.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this person's information.");
            }
        }
    }
}
