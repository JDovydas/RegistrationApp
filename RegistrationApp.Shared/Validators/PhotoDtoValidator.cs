using FluentValidation;
using Microsoft.AspNetCore.Http;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.Shared.Validators
{
    public class PhotoDtoValidator : AbstractValidator<PhotoDto>
    {
        public PhotoDtoValidator()

        {
            RuleFor(x => x.ProfilePhoto)
                .NotEmpty()
                .Must(BeAValidProfilePhoto).WithMessage("Profile photo must be a valid image file.")
                .Must(BeAValidSize).WithMessage("Profile photo must not exceed 5MB.");
        }
        private bool BeAValidProfilePhoto(IFormFile profilePhoto)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".jfif" };
            var extension = Path.GetExtension(profilePhoto.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
        private bool BeAValidSize(IFormFile profilePhoto)
        {
            const int maxSizeInBytes = 5 * 1024 * 1024; // 5MB in bytes
            return profilePhoto.Length <= maxSizeInBytes;
        }
    }
}
