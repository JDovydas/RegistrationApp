using FluentValidation;
using RegistrationApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Database.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(8, 20).WithMessage("Username must be between 8 and 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(12).WithMessage("Password must be at least 12 characters long.")
                .Must(ContainAtLeastTwoUppercase).WithMessage("Password must contain at least two uppercase letters.")
                .Must(ContainAtLeastTwoLowercase).WithMessage("Password must contain at least two lowercase letters.")
                .Must(ContainAtLeastTwoNumbers).WithMessage("Password must contain at least two numbers.")
                .Must(ContainAtLeastTwoSpecialCharacters).WithMessage("Password must contain at least two special characters.");
        }

        private bool ContainAtLeastTwoUppercase(string password)
        {
            return password.Count(char.IsUpper) >= 2;
        }

        private bool ContainAtLeastTwoLowercase(string password)
        {
            return password.Count(char.IsLower) >= 2;
        }

        private bool ContainAtLeastTwoNumbers(string password)
        {
            return password.Count(char.IsDigit) >= 2;
        }

        private bool ContainAtLeastTwoSpecialCharacters(string password)
        {
            return password.Count(c => !char.IsLetterOrDigit(c)) >= 2;
        }
    }
}
