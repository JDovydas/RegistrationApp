using FluentValidation;
using RegistrationApp.Shared.DTOs;
namespace RegistrationApp.Shared.Validators
{
    public class PlaceOfResidenceDtoValidator : AbstractValidator<PlaceOfResidenceDto>
    {
        public PlaceOfResidenceDtoValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("City must contain only letters and spaces. Space is allowed between words only.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("Street must contain only letters and spaces. Space is allowed between words only.");

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithMessage("House number is required.")
                .GreaterThan(0).WithMessage("House Number must be a positive integer.");

            RuleFor(x => x.AppartmentNumber)
                .GreaterThan(0).WithMessage("Apartment Number must be a number.");
        }
    }
}
