using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models.Handlers
{
    public class EmployeeEmailHandler : AuthorizationHandler<EmployeeEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeEmailRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "Employee"))
            {
                return Task.CompletedTask;
            }
            
            string[] email = context.User.FindFirst(c => c.Type == "Employee").Value.Split("@");
            if(email[1] == requirement.EmployeeEmail)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
