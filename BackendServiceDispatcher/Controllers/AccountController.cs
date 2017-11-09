using BackendServiceDispatcher.Models.AccountEntities;
using BackendServiceDispatcher.Models.AccountDataModels;
using BackendServiceDispatcher.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendServiceDispatcher.Extensions;

namespace BackendServiceDispatcher.Controllers
{
    /// <summary>
    /// Account Endpoint
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// A Test GET API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]        
        public IActionResult Get()
        {
            return new OkObjectResult("api/Account/");
        }

        /// <summary>
        /// User Registration API
        /// </summary>
        /// <param name="registrationDataModel"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody]RegistrationDataModel registrationDataModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser applicationUser = new ApplicationUser() { UserName = registrationDataModel.Email, Email = registrationDataModel.Email };
            var result = await _userManager.CreateAsync(applicationUser, registrationDataModel.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var callbackUrl = Url.EmailConfirmationLink(applicationUser.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(registrationDataModel.Email, callbackUrl);
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                return new OkResult();
            }
            return new BadRequestObjectResult(result.Errors);            
        }

        /// <summary>
        /// Email Confirmation API
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new BadRequestObjectResult("userId and code are required");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return new OkResult();
        }
    }
}
