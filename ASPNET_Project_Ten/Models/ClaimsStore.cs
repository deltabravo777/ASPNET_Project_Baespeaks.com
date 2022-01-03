using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Create Role", "Create Role"),
            new Claim("Edit Role", "Edit Role"),
            new Claim("Delete Role", "Delete Role"),
            new Claim("Manager Claim", "Manager Claim"),
            new Claim("Administrative Claim", "Administrative Claim"),
            new Claim("Executive Claim", "Executive Claim")
        };
    }
}
