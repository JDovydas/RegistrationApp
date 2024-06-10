using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.BusinessLogic.Helpers;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;

namespace RegistrationApp.BusinessLogic.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlaceOfResidenceRepository _placeOfResidenceRepository;

        // Constructor to initialize repositories
        public PersonService(IPersonRepository personRepository, IUserRepository userRepository, IPlaceOfResidenceRepository placeOfResidenceRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _placeOfResidenceRepository = placeOfResidenceRepository;
        }

        public async Task AddPersonInformationAsync(Guid userId, PersonDto personDto, PlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist");
            }

            var placeOfResidence = new PlaceOfResidence
            {
                Id = Guid.NewGuid(),
                City = placeOfResidenceDto.City,
                Street = placeOfResidenceDto.Street,
                HouseNumber = placeOfResidenceDto.HouseNumber,
                AppartmentNumber = placeOfResidenceDto.AppartmentNumber
            };

            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = personDto.Name,
                LastName = personDto.LastName,
                Gender = personDto.Gender,
                BirthDate = birthDate,
                PersonalId = personDto.PersonalId,
                PhoneNumber = personDto.PhoneNumber,
                Email = personDto.Email,
                FilePath = filePath,
                PlaceOfResidenceId = placeOfResidence.Id,
                UserId = user.Id,
            };

            await _personRepository.AddPersonAsync(person);

            placeOfResidence.PersonId = person.Id;

            await _placeOfResidenceRepository.AddPlaceOfResidenceAsync(placeOfResidence);
        }

        public async Task<string> UploadProfilePhotoAsync(IFormFile profilePhoto)
        {
            return await ProfilePhotoHelpers.SaveProfilePhotoAsync(profilePhoto);
        }

        public bool ValitateBirthDate(string birthDateString, out DateOnly birthDate)
        {
            return PersonInformationHelpers.TryParseBirthDate(birthDateString, out birthDate);
        }

        public async Task UpdateNameAsync(Guid userId, Guid personId, string newName)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.Name = newName;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdateLastNameAsync(Guid userId, Guid personId, string newLastName)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.LastName = newLastName;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdateGenderAsync(Guid userId, Guid personId, string newGender)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.Gender = newGender;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdateBirthDateAsync(Guid userId, Guid personId, DateOnly newBirthDate)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.BirthDate = newBirthDate;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdateIdNumberAsync(Guid userId, Guid personId, string newPersonalId)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.PersonalId = newPersonalId;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdatePhoneNumberAsync(Guid userId, Guid personId, string newPhoneNumber)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.PhoneNumber = newPhoneNumber;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdateEmailAsync(Guid userId, Guid personId, string newEmail)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);
            person.Email = newEmail;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task UpdatePhotoAsync(Guid userId, Guid personId, IFormFile newProfilePhoto)
        {
            // Ensure person is available and user owns it
            await PersonInformationHelpers.EnsureUserOwnsPersonAsync(_personRepository, userId, personId);

            var person = await _personRepository.GetPersonByIdAsync(personId);

            // Delete the old photo if it exists
            if (!string.IsNullOrEmpty(person.FilePath))
            {
                ProfilePhotoHelpers.DeleteProfilePhoto(person.FilePath);
            }

            // Save the new photo
            var filePath = await ProfilePhotoHelpers.SaveProfilePhotoAsync(newProfilePhoto);
            person.FilePath = filePath;
            await _personRepository.UpdatePersonAsync(person);
        }

        public async Task DeletePersonByIdAsync(Guid personId)
        {
            var personToDelete = await _personRepository.GetPersonByIdAsync(personId);

            if (personToDelete == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            // Delete the photo if it exists
            if (!string.IsNullOrEmpty(personToDelete.FilePath))
            {
                ProfilePhotoHelpers.DeleteProfilePhoto(personToDelete.FilePath);
            }

            await _personRepository.DeletePersonAsync(personToDelete);
        }

        public async Task<RetrievePersonInformationDto> RetrievePersonInformationAsync(Guid userId, Guid personId)
        {
            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this person's information.");
            }

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(personId);

            var fullPersonInformation = new RetrievePersonInformationDto
            {
                Name = person.Name,
                LastName = person.LastName,
                Gender = person.Gender,
                BirthDate = person.BirthDate.ToString(),
                PersonalId = person.PersonalId,
                PhoneNumber = person.PhoneNumber,
                Email = person.Email,
                City = placeOfResidence.City,
                Street = placeOfResidence.Street,
                HouseNumber = placeOfResidence.HouseNumber,
                AppartmentNumber = placeOfResidence.AppartmentNumber,
            };
            return fullPersonInformation;
        }

        public async Task<FileContentResult> RetrievePersonProfilePhotoAsync(Guid userId, Guid personId)
        {
            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this person's information.");
            }

            var filePath = person.FilePath;
            return await ProfilePhotoHelpers.GetProfilePhotoAsync(filePath);
        }
    }
}
