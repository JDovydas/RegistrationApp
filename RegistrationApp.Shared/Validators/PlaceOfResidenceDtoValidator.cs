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
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž \\-]+$").WithMessage("City must contain only letters, spaces and hyphens.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž \\-]+$").WithMessage("Street must be valid.");

            RuleFor(x => x.HouseNumber)
                .GreaterThan(0).WithMessage("House Number must be a positive integer.");

            RuleFor(x => x.AppartmentNumber)
                .GreaterThanOrEqualTo(0).When(x => x.AppartmentNumber.HasValue).WithMessage("Apartment Number must be a number.");
        }
    }
}
