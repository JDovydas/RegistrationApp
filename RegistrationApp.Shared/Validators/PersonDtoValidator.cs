using FluentValidation;
using Microsoft.AspNetCore.Http;
using RegistrationApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var extension = Path.GetExtension(profilePhoto.FileName).ToLower();
            return allowedExtensions.Contains(extension);
        }

        //private bool BeAValidPersonalId(string personalId)
        //{
        //    // Validate length
        //    if (personalId.Length != 11)
        //    {
        //        return false;
        //    }

        //    // Validate format (only digits)
        //    if (!Regex.IsMatch(personalId, @"^\d{11}$"))
        //    {
        //        return false;
        //    }

        //    // Extract parts of the personal code
        //    int centuryAndGender = int.Parse(personalId.Substring(0, 1));
        //    int year = int.Parse(personalId.Substring(1, 2));
        //    int month = int.Parse(personalId.Substring(3, 2));
        //    int day = int.Parse(personalId.Substring(5, 2));
        //    int serialNumber = int.Parse(personalId.Substring(7, 3));
        //    int checkDigit = int.Parse(personalId.Substring(10, 1));

        //    // Validate birth date
        //    try
        //    {
        //        int fullYear = GetFullYear(centuryAndGender, year);
        //        var birthDate = new DateTime(fullYear, month, day);
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //    // Validate check digit
        //    return checkDigit == CalculateCheckDigit(personalId);
        //}

        //private int GetFullYear(int centuryAndGender, int year)
        //{
        //    if (centuryAndGender >= 1 && centuryAndGender <= 2)
        //    {
        //        return 1800 + year;
        //    }
        //    if (centuryAndGender >= 3 && centuryAndGender <= 4)
        //    {
        //        return 1900 + year;
        //    }
        //    if (centuryAndGender >= 5 && centuryAndGender <= 6)
        //    {
        //        return 2000 + year;
        //    }
        //    if (centuryAndGender >= 7 && centuryAndGender <= 8)
        //    {
        //        return 2100 + year;
        //    }
        //    throw new ArgumentException("Invalid century and gender indicator in personal code.");
        //}

        //private int CalculateCheckDigit(string personalId)
        //{
        //    int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2 };
        //    int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4 };

        //    int sum = 0;
        //    for (int i = 0; i < 10; i++)
        //    {
        //        sum += (personalId[i] - '0') * weights1[i];
        //    }

        //    int remainder = sum % 11;
        //    if (remainder == 10)
        //    {
        //        sum = 0;
        //        for (int i = 0; i < 10; i++)
        //        {
        //            sum += (personalId[i] - '0') * weights2[i];
        //        }
        //        remainder = sum % 11;
        //        if (remainder == 10)
        //        {
        //            remainder = 0;
        //        }
        //    }

        //    return remainder;
        //}
    }
}
