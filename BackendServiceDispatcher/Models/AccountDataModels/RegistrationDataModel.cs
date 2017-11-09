using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using BackendServiceDispatcher.Models.AccountDataModels.Validations;

namespace BackendServiceDispatcher.Models.AccountDataModels
{
    [Validator(typeof(RegistrationViewModelValidator))]
    public class RegistrationDataModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
