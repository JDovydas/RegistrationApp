using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;


namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPersonService
    {
        Task AddPersonInformation(Guid userId, PersonDto personDto, PlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate);
        Task UpdateName(Guid userId, Guid personId, string newName);
        Task UpdateLastName(Guid userId, Guid personId, string newLastName);
        Task UpdateGender(Guid userId, Guid personId, string newGender);
        Task UpdateBirthDate(Guid userId, Guid personId, DateOnly newBirthDate);
        Task UpdateIdNumber(Guid userId, Guid personId, string newPersonalIdNumber);
        Task UpdatePhoneNumber(Guid userId, Guid personId, string newPhoneNumber);
        Task UpdateEmail(Guid userId, Guid personId, string newEmail);
        Task UpdatePhoto(Guid userId, Guid personId, string newFilePath);

    }
}
