using FluentValidation;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.Shared.Validators
{
    public class UpdatePlaceOfResidenceDtoValidator : AbstractValidator<UpdatePlaceOfResidenceDto>
    {
        public UpdatePlaceOfResidenceDtoValidator()
        {
            RuleFor(x => x.City)
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("City must contain only letters and spaces. Space is allowed between words only.")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.Street)
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("Street must contain only letters and spaces. Space is allowed between words only.")
                .When(x => !string.IsNullOrEmpty(x.Street));

            RuleFor(x => x.HouseNumber)
                .GreaterThan(0).WithMessage("House Number must be a positive integer.");

            RuleFor(x => x.AppartmentNumber)
                .GreaterThan(0).WithMessage("Apartment Number must be a number.");
        }
    }
}
