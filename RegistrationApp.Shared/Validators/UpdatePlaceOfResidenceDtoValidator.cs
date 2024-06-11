using FluentValidation;
using RegistrationApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Shared.Validators
{
    public class UpdatePlaceOfResidenceDtoValidator : AbstractValidator<UpdatePlaceOfResidenceDto>
    {
        public UpdatePlaceOfResidenceDtoValidator()
        {
            RuleFor(x => x.City)
                //.Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž \\-]+$").WithMessage("City must contain only letters, spaces and hyphens.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("City must contain only letters and spaces. Spaced are allowed between words only.")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.Street)
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+(?: [a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+)*$").WithMessage("Street must contain only letters and spaces. Spaced are allowed between words only.")
                .When(x => !string.IsNullOrEmpty(x.Street));

            RuleFor(x => x.HouseNumber)
                .GreaterThan(0).When(x => x.HouseNumber.HasValue).WithMessage("House Number must be a positive integer.");

            RuleFor(x => x.AppartmentNumber)
                .GreaterThanOrEqualTo(0).When(x => x.AppartmentNumber.HasValue).WithMessage("Apartment Number must be a number.");
        }
    }

}
