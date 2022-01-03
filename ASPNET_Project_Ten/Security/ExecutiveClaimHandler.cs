using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASPNET_Project_Eleven.Security
{
    public class ExecutiveClaimHandler : AuthorizationHandler<ExecutiveClaimsRequirement>, IAuthorizationHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExecutiveClaimHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExecutiveClaimsRequirement requirement)
        {
            if (    context.User.HasClaim(c => c.Type == "Executive Claim" && c.Value == "true")
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

