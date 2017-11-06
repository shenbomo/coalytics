using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendServiceDispatcher.Models.AccountViewModels;

namespace BackendServiceDispatcher.Models.AccountViewModels.Validations
{
    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(vm => vm.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(vm => vm.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .Length(6, 12).WithMessage("Password must be between 6 and 12 characters");

            RuleFor(vm => vm.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password")
                .Equal(vm => vm.Password).WithMessage("Passwords do not match");
        }
    }
}
