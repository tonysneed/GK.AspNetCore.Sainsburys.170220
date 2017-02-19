using System.Threading.Tasks;
using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Authorization.Policies
{
    public class SalesViewPolicy : AuthorizationHandler
        <OperationAuthorizationRequirement, SalesData>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, SalesData resource)
        {
            if (requirement == SalesOperations.View)
            {
                if (context.User.HasClaim("role", "Admin") ||
                    context.User.HasClaim("clearance", "WikiLeaks") ||
                    context.User.HasClaim("role", "Sales") &&
                    context.User.HasClaim("region", resource.Region))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.FromResult(0);
        }
    }
}
