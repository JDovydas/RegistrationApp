using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Shared.DTOs;
using RegistrationApp.Shared.Models;


namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPersonService
    {
        Task AddPersonInformationAsync(Guid userId, PersonDto personDto, PlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate);
        Task UpdateNameAsync(Guid userId, Guid personId, string newName);
        Task UpdateLastNameAsync(Guid userId, Guid personId, string newLastName);
        Task UpdateGenderAsync(Guid userId, Guid personId, string newGender);
        Task UpdateBirthDateAsync(Guid userId, Guid personId, DateOnly newBirthDate);
        Task UpdateIdNumberAsync(Guid userId, Guid personId, string newPersonalIdNumber);
        Task UpdatePhoneNumberAsync(Guid userId, Guid personId, string newPhoneNumber);
        Task UpdateEmailAsync(Guid userId, Guid personId, string newEmail);
        Task UpdatePhotoAsync(Guid userId, Guid personId, IFormFile newProfilePhoto);
        Task DeletePersonByIdAsync(Guid userId);
        Task<RetrievePersonInformationDto> RetrievePersonInformationAsync(Guid userId, Guid personId);
        bool ValitateBirthDate(string birthDateString, out DateOnly birthDate);
        Task<string> ProfilePhotoUploadAsync(IFormFile profilePhoto);
        Task<FileContentResult> RetrievePersonProfilePhotoAsync(Guid userId, Guid personId);
    }
}
