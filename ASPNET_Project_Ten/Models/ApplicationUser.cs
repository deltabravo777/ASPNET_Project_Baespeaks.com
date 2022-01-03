using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        public int? BirthYear { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthSingleDay { get; set; }
    }
}
