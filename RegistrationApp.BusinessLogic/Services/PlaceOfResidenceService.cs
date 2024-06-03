﻿using RegistrationApp.Database.Repositories.Interfaces;
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
    internal class PlaceOfResidenceService : IPlaceOfResidenceService
    {
        private readonly IPlaceOfResidenceRepository _placeOfResidenceRepository;
        private readonly IPersonRepository _personRepository;


        public PlaceOfResidenceService(IPlaceOfResidenceRepository placeOfResidenceRepository, IPersonRepository personRepository)
        {
            _placeOfResidenceRepository = placeOfResidenceRepository;
            _personRepository = personRepository;
        }

        public async Task<PlaceOfResidenceDto> UpdateCity(Guid userId, Guid personId, string newCity)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
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

            await _placeOfResidenceRepository.UpdatePlaceOfResidence(placeOfResidence);

            var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            {
                City = placeOfResidence.City,
                Street = placeOfResidence.Street,
                HouseNumber = placeOfResidence.HouseNumber,
                AppartmentNumber = placeOfResidence.AppartmentNumber
            };

            return updatedPlaceOfResidenceDto;
        }

        public async Task<PlaceOfResidenceDto> UpdateStreet(Guid userId, Guid personId, string newStreet)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
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

            await _placeOfResidenceRepository.UpdatePlaceOfResidence(placeOfResidence);

            var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            {
                City = placeOfResidence.City,
                Street = placeOfResidence.Street,
                HouseNumber = placeOfResidence.HouseNumber,
                AppartmentNumber = placeOfResidence.AppartmentNumber
            };

            return updatedPlaceOfResidenceDto;
        }

        public async Task<PlaceOfResidenceDto> UpdateHouseNumber(Guid userId, Guid personId, int newHouseNumber)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
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

            await _placeOfResidenceRepository.UpdatePlaceOfResidence(placeOfResidence);

            var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            {
                City = placeOfResidence.City,
                Street = placeOfResidence.Street,
                HouseNumber = placeOfResidence.HouseNumber,
                AppartmentNumber = placeOfResidence.AppartmentNumber
            };

            return updatedPlaceOfResidenceDto;

        }

        public async Task<PlaceOfResidenceDto> UpdateAppartmentNumber(Guid userId, Guid personId, int newApparmentNumber)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
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

            await _placeOfResidenceRepository.UpdatePlaceOfResidence(placeOfResidence);

            var updatedPlaceOfResidenceDto = new PlaceOfResidenceDto
            {
                City = placeOfResidence.City,
                Street = placeOfResidence.Street,
                HouseNumber = placeOfResidence.HouseNumber,
                AppartmentNumber = placeOfResidence.AppartmentNumber
            };

            return updatedPlaceOfResidenceDto;
        }

        public async Task EnsureUserOwnsPerson(Guid userId, Guid personId) /// Duplicate method should it be added to Helpers? Is it OK that Task does not contain a  model in itself?
        {
            var person = await _personRepository.GetPersonById(personId);
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
