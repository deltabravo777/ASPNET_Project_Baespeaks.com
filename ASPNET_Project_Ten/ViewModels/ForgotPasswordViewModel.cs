using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

