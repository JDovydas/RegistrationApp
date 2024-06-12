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

        public async Task UpdatePersonInformationAsync(Guid personId, Guid userId, UpdatePersonDto personDto, UpdatePlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate)
        {
            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null || person.UserId != userId)
            {
                throw new InvalidOperationException("Person not found or unauthorized access.");
            }

            // Update person properties if they are not null
            if (!string.IsNullOrEmpty(personDto.Name)) person.Name = personDto.Name;
            if (!string.IsNullOrEmpty(personDto.LastName)) person.LastName = personDto.LastName;
            if (!string.IsNullOrEmpty(personDto.Gender)) person.Gender = personDto.Gender;
            if (birthDate != default) person.BirthDate = birthDate;
            if (!string.IsNullOrEmpty(personDto.PersonalId)) person.PersonalId = personDto.PersonalId;
            if (!string.IsNullOrEmpty(personDto.PhoneNumber)) person.PhoneNumber = personDto.PhoneNumber;
            if (!string.IsNullOrEmpty(personDto.Email)) person.Email = personDto.Email;
            if (!string.IsNullOrEmpty(filePath))
            {
                // Delete the old photo if it exists
                if (!string.IsNullOrEmpty(person.FilePath))
                {
                    ProfilePhotoHelpers.DeleteProfilePhoto(person.FilePath);
                }
                person.FilePath = filePath;
            }

            await _personRepository.UpdatePersonAsync(person);

            var placeOfResidence = await _placeOfResidenceRepository.GetPlaceOfResidenceByPersonIdAsync(person.Id);

            // Update place of residence properties if they are not null
            if (!string.IsNullOrEmpty(placeOfResidenceDto.City)) placeOfResidence.City = placeOfResidenceDto.City;
            if (!string.IsNullOrEmpty(placeOfResidenceDto.Street)) placeOfResidence.Street = placeOfResidenceDto.Street;
            if (placeOfResidenceDto.HouseNumber.HasValue) placeOfResidence.HouseNumber = placeOfResidenceDto.HouseNumber.Value;
            if (placeOfResidenceDto.AppartmentNumber.HasValue) placeOfResidence.AppartmentNumber = placeOfResidenceDto.AppartmentNumber.Value;

            await _placeOfResidenceRepository.UpdatePlaceOfResidenceAsync(placeOfResidence);
        }

        public async Task<string> UploadProfilePhotoAsync(IFormFile profilePhoto)
        {
            return await ProfilePhotoHelpers.SaveProfilePhotoAsync(profilePhoto);
        }

        public bool ValitateBirthDate(string birthDateString, out DateOnly birthDate)
        {
            return PersonInformationHelpers.TryParseBirthDate(birthDateString, out birthDate);
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
