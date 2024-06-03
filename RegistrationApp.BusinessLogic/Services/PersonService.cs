using RegistrationApp.BusinessLogic.Services.Interfaces;
using RegistrationApp.Database.Repositories.Interfaces;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.BusinessLogic.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlaceOfResidenceRepository _placeOfResidenceRepository;

        public PersonService(IPersonRepository personRepository, IUserRepository userRepository, IPlaceOfResidenceRepository placeOfResidenceRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _placeOfResidenceRepository = placeOfResidenceRepository;
        }

        public async Task AddPersonInformation(Guid userId, PersonDto personDto, PlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate)
        {
            var user = await _userRepository.GetUserById(userId);
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
                UserId = user.Id,
                PlaceOfResidenceId = placeOfResidence.Id, // Set foreign key
                PlaceOfResidence = placeOfResidence // Assign the related entity
            };

            //var person = new Person
            //{
            //    Id = Guid.NewGuid(),
            //    Name = personDto.Name,
            //    LastName = personDto.LastName,
            //    Gender = personDto.Gender,
            //    BirthDate = birthDate,
            //    PersonalId = personDto.PersonalId,
            //    PhoneNumber = personDto.PhoneNumber,
            //    Email = personDto.Email,
            //    FilePath = filePath,
            //    UserId = user.Id
            //};

            //var placeOfResidence = new PlaceOfResidence /// Should I move it outsde - to PlaceOfResidenceService?
            //{
            //    Id = Guid.NewGuid(),
            //    City = placeOfResidenceDto.City,
            //    Street = placeOfResidenceDto.Street,
            //    HouseNumber = placeOfResidenceDto.HouseNumber,
            //    AppartmentNumber = placeOfResidenceDto.AppartmentNumber,
            //    Person = person
            //};

            await _placeOfResidenceRepository.AddPlaceOfResidence(placeOfResidence);
            await _personRepository.AddPerson(person);
        }

        public async Task UpdateName(Guid userId, Guid personId, string newName) // Should I user just Task instead?
        {
            await EnsureUserOwnsPerson(userId, personId); // DOES THIS STOP PROCEEDING WITH THE METHOD FURTHER?

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            person.Name = newName;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdateLastName(Guid userId, Guid personId, string newlastName) // Should I user just Task instead?
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }

            person.LastName = newlastName;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdateGender(Guid userId, Guid personId, string newGender) // Should I user just Task instead?
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.Gender = newGender;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdateBirthDate(Guid userId, Guid personId, DateOnly newBirthDate) // Should I user just Task instead?
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.BirthDate = newBirthDate;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdateIdNumber(Guid userId, Guid personId, string newPersonalId)// Should I user just Task instead?
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.PersonalId = newPersonalId;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdatePhoneNumber(Guid userId, Guid personId, string newPhoneNumber)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.PhoneNumber = newPhoneNumber;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdateEmail(Guid userId, Guid personId, string newEmail)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.Email = newEmail;

            await _personRepository.UpdatePerson(person);
        }

        public async Task UpdatePhoto(Guid userId, Guid personId, string newFilePath)
        {
            await EnsureUserOwnsPerson(userId, personId);

            var person = await _personRepository.GetPersonById(personId);
            if (person == null)
            {
                throw new InvalidOperationException("Person not found.");
            }
            person.FilePath = newFilePath;

            await _personRepository.UpdatePerson(person);
        }

        public async Task EnsureUserOwnsPerson(Guid userId, Guid personId) /// should it be added to Helpers? Is it OK that Task does not contain a  model in itself?
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
