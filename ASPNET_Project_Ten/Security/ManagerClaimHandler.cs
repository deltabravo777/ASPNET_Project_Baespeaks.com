using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Security
{
    public class ManagerClaimHandler : AuthorizationHandler<ManagerClaimsRequirement>, IAuthorizationHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ManagerClaimHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManagerClaimsRequirement requirement)
        {
            /*
            new Claim("Manager Claim", "Manager Claim"),
            new Claim("Administrative Claim", "Administrative Claim"),
            new Claim("Executive Claim", "Executive Claim")

            new Claim("Manager Claim", "Manager Claim"),
            new Claim("Administrative Claim", "Administrative Claim"),
            new Claim("Executive Claim", "Executive Claim")
            */
            if (    context.User.HasClaim(c => c.Type == "Manager Claim"            && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Administrative Claim"     && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Executive Claim"          && c.Value == "true")
            )
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

}

