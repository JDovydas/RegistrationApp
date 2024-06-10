using FluentValidation;
using Microsoft.AspNetCore.Http;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.Shared.Validators
{
    public class PersonDtoValidator : AbstractValidator<PersonDto>
    {
        public PersonDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Name must contain only letters.") //no spaces/dashes are allowed 
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Last Name must contain only letters.")
                .Length(2, 50).WithMessage("Last Name must be between 2 and 50 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required.")
                .Must(BeAValidDate).WithMessage("Invalid date format for BirthDate. Please use YYYY-MM-DD.")
                .Must(BeAValidAge).WithMessage("BirthDate cannot be in the future and must be within the last 120 years.");

            RuleFor(x => x.PersonalId)
                .NotEmpty().WithMessage("Personal Identification Code is required.")
                .Matches(@"^\d{11}$").WithMessage("Personal Identification Code must be 11 digits long.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^(?:\+3706\d{7}|06\d{7})$").WithMessage("Phone Number must be in the format +3706XXXXXXX or 06XXXXXXX."); //?: - allows either of the patterns

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.ProfilePhoto)
                .NotNull().WithMessage("Profile Photo is required.")
                .Must(BeAValidProfilePhoto).WithMessage("Profile photo must be a valid image file.");
        }

        private bool BeAValidDate(string birthDateString)
        {
            return DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out _); //TryParseExact to validate the format and ignores the out parameter using _.
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
    }
}
