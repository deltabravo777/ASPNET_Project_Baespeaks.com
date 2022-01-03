using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_Project_Eleven.Utilities;
using ASPNET_Project_Eleven.Models;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [ValidationEmailDomain(allowedDomain: "baespeaks.com", ErrorMessage = "Email domain must be baespeaks.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }

        [Display(Name = "Birth Year")]
        public int? BirthYear { get; set; }
        [Display(Name = "Birth Month")]
        public int? BirthMonth { get; set; }
        [Display(Name = "Birth Date")]
        public int? BirthSingleDay { get; set; }
    }
}

