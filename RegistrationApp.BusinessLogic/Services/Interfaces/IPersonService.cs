using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.BusinessLogic.Services.Interfaces
{
    public interface IPersonService
    {
        Task AddPersonInformationAsync(Guid userId, PersonDto personDto, PlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate);
        Task UpdatePersonInformationAsync(Guid personId, Guid userId, UpdatePersonDto personDto, UpdatePlaceOfResidenceDto placeOfResidenceDto, string filePath, DateOnly birthDate);
        Task DeletePersonByIdAsync(Guid userId);
        Task<RetrievePersonInformationDto> RetrievePersonInformationAsync(Guid userId, Guid personId);
        bool ValitateBirthDate(string birthDateString, out DateOnly birthDate);
        Task<string> UploadProfilePhotoAsync(IFormFile profilePhoto);
        Task<FileContentResult> RetrievePersonProfilePhotoAsync(Guid userId, Guid personId);
    }
}
