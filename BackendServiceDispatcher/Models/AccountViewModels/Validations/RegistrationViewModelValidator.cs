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
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Email cannot be null")
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(vm => vm.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Password cannot be null")
                .NotEmpty().WithMessage("Password cannot be empty")
                .Length(6, 12).WithMessage("Password must be between 6 and 12 characters");

            RuleFor(vm => vm.ConfirmPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Password cannot be null")
                .NotEmpty().WithMessage("Please confirm your password")
                .Equal(vm => vm.Password).WithMessage("Passwords do not match");
        }
    }
}
