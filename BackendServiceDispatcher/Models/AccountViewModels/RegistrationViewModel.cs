using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using BackendServiceDispatcher.Models.AccountViewModels.Validations;

namespace BackendServiceDispatcher.Models.AccountViewModels
{
    [Validator(typeof(RegistrationViewModelValidator))]
    public class RegistrationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
