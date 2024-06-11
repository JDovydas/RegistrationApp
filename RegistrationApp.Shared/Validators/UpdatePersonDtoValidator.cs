using FluentValidation;
using Microsoft.AspNetCore.Http;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.Shared.Validators
{
    public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
    {
        public UpdatePersonDtoValidator()
        {
            RuleFor(x => x.Name)
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Name must contain only letters.")
                .Length(2, 50).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Last Name must contain only letters.")
                .Length(2, 50).When(x => !string.IsNullOrEmpty(x.LastName)).WithMessage("Last Name must be between 2 and 50 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required").When(x => !string.IsNullOrEmpty(x.Gender));

            RuleFor(x => x.BirthDate)
                .Must(BeAValidDate).WithMessage("Invalid date format for BirthDate. Please use YYYY-MM-DD.")
                .Must(BeAValidAge).WithMessage("BirthDate cannot be in the future and must be within the last 120 years.")
                .When(x => !string.IsNullOrEmpty(x.BirthDate));

            RuleFor(x => x.PersonalId)
                .Matches(@"^\d{11}$").WithMessage("Personal Identification Code must be 11 digits long.")
                .When(x => !string.IsNullOrEmpty(x.PersonalId));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(?:\+3706\d{7}|06\d{7})$").WithMessage("Phone Number must be in the format +3706XXXXXXX or 06XXXXXXX.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.ProfilePhoto)
                .Must(BeAValidProfilePhoto).WithMessage("Profile photo must be a valid image file.")
                .Must(BeAValidSize).WithMessage("Profile photo must not exceed 5MB.")
                .When(x => x.ProfilePhoto != null);
        }

        private bool BeAValidDate(string birthDateString)
        {
            return DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out _);
        }

        private bool BeAValidAge(string birthDateString)
        {
            if (!DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out DateOnly birthDate))
            {
                return false;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var maxDate = today.AddYears(-120);

            return birthDate <= today && birthDate >= maxDate;
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
