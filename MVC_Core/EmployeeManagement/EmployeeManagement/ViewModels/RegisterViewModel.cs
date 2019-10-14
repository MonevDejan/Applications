using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]

        // We use Remote atribute to provide remote validation for an email
        // We use 3 scripts from jquery to achieve this
        [Remote(action: "IsEmailInUse", controller: "Account")]

        // Own custom validation attribute
        // this validation is only on server. we need to change jquery sctripts to catch this attribute 
        [ValidEmailDomain(allowedDomain: "dejantech.com",ErrorMessage = "Email domain must be dejantech.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
