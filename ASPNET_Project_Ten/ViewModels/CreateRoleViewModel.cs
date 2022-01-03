using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

