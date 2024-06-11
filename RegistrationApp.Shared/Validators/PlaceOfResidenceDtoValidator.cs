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
                //[a - zA - ZĄČĘĖĮŠŲŪŽąčęėįšųūž] Ensures the city name starts with a letter.
                //(?: [a - zA - ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)$ :Allows spaces followed by letters or hyphens in 
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("City must contain only letters and spaces. Spaced are allowed between words only.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("Street must contain only letters and spaces. Spaced are allowed between words only.");

            RuleFor(x => x.HouseNumber)
                .GreaterThan(0).WithMessage("House Number must be a positive integer.");

            RuleFor(x => x.AppartmentNumber)
                //.GreaterThan(0).When(x => x.AppartmentNumber.HasValue).WithMessage("Apartment Number must be a number.");
                .GreaterThan(0).WithMessage("Apartment Number must be a number.");
        }
    }
}
