using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Validators
{
    /// <summary>
    /// In some scenarios you want to apply your own custom policy for validating email, or password.
    /// This can be done easily by creating your own validation classes 
    /// and hock it to “UserValidator” and “PasswordValidator” properties 
    /// in class “ApplicationUserManager”.
    /// For example if we want to enforce using only the following domains 
    /// (“outlook.com”, “hotmail.com”, “gmail.com”, “yahoo.com”) 
    /// when the user self registers then we need to create a class and derive it from 
    /// UserValidator
    /// </summary>
    public class MyCustomUserValidator : UserValidator<ApplicationUser>
    {
        readonly List<string> _allowedEmailDomains = new List<string>
        {
            "outlook.com", "hotmail.com", "gmail.com", "yahoo.com"
        };

        public MyCustomUserValidator(ApplicationUserManager appUserManager)
            : base(appUserManager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add($"Email domain '{emailDomain}' is not allowed");

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}