using FluentValidation; //.NET library for building strongly-typed validation rules. Creates validation rules for classes, usually DTOs or models. 
using Microsoft.AspNetCore.Http;
using RegistrationApp.Shared.DTOs;

namespace RegistrationApp.Shared.Validators
{
    public class PersonDtoValidator : AbstractValidator<PersonDto>
    {
        public PersonDtoValidator()
        {
            //RuleFor method - specifies property to validate and conditions it must satisfy.
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Name must contain only letters.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Matches("^[a-zA-ZĄČĘĖĮŠŲŪŽąčęėįšųūž]+$").WithMessage("Last Name must contain only letters.")
                .Length(2, 50).WithMessage("Last Name must be between 2 and 50 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required.")
                .Must(BeAValidDate).WithMessage("Invalid date format for BirthDate. Please use YYYY-MM-DD.") //custom validators use "Must" method.
                .Must(BeAValidAge).WithMessage("BirthDate cannot be in the future and must be within the last 120 years.");

            RuleFor(x => x.PersonalId)
                .NotEmpty().WithMessage("Personal Identification Code is required.")
                .Matches(@"^\d{11}$").WithMessage("Personal Identification Code must be 11 digits long.")

                .Must(BeAValidpPersonalId).WithMessage("Invalid Personal Identification Code.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^(?:\+3706\d{7}|06\d{7})$").WithMessage("Phone Number must be in the format +3706XXXXXXX or 06XXXXXXX.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.ProfilePhoto)
                .NotNull().WithMessage("Profile Photo is required.")
                .Must(BeAValidProfilePhoto).WithMessage("Profile photo must be a valid image file.")
                .Must(BeAValidSize).WithMessage("Profile photo must not exceed 5MB.");
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

        private bool BeAValidpPersonalId(string personalId)
        {
            //Kontrolinis skaičius
            //Jei asmens kodas užrašomas ABCDEFGHIJK, tai:
            //S = A * 1 + B * 2 + C * 3 + D * 4 + E * 5 + F * 6 + G * 7 + H * 8 + I * 9 + J * 1
            //Suma S dalinama iš 11, ir jei liekana nelygi 10, ji yra asmens kodo kontrolinis skaičius K.
            //Jei liekana lygi 10, tuomet skaičiuojama nauja suma su tokiais svertiniais koeficientais:
            //S = A * 3 + B * 4 + C * 5 + D * 6 + E * 7 + F * 8 + G * 9 + H * 1 + I * 2 + J * 3
            //Ši suma S vėl dalinama iš 11, ir jei liekana nelygi 10, ji yra asmens kodo kontrolinis skaičius K.
            //Jei vėl liekana yra 10, kontrolinis skaičius K yra 0.

            int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

            int sum = 0;

            for (int i = 0; i < 10; i++)
            {
                //personalId[i] is a character from the personal identification code
                //personalId[i] - '0' converts that character to its numeric value (The ASCII value of '0' is 48)

                sum += (personalId[i] - '0') * weights1[i];
            }

            int remainder = sum % 11;
            if (remainder == 10)
            {
                sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    sum += (personalId[i] - '0') * weights2[i];
                }
                remainder = sum % 11;
                if (remainder == 10)
                {
                    remainder = 0;
                }
            }

            int expectedCheckDigit = remainder;
            int actualCheckDigit = personalId[10] - '0';

            return expectedCheckDigit == actualCheckDigit;
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


