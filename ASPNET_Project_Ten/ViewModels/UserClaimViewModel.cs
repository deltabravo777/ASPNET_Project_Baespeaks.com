using System;
using System.Collections.Generic;
using ASPNET_Project_Eleven.Models;

namespace ASPNET_Project_Eleven.ViewModels
{
    public class UserClaimViewModel
    {
        public UserClaimViewModel()
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}

