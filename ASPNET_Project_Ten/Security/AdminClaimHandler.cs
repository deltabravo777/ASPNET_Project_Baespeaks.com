using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Security
{
    public class AdminClaimHandler : AuthorizationHandler<AdminClaimsRequirement>, IAuthorizationHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AdminClaimHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminClaimsRequirement requirement)
        {
            if (    context.User.HasClaim(c => c.Type == "Administrative Claim" && c.Value == "true") ||
                    context.User.HasClaim(c => c.Type == "Executive Claim" && c.Value == "true")
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

