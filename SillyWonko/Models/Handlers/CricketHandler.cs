using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models.Handlers
{
    public class CricketHandler : AuthorizationHandler<CricketRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CricketRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "BuzzyFizz"))
            {
                return Task.CompletedTask;
            }

            string cricket = context.User.FindFirst(c => c.Type == "BuzzyFizz").Value;
            
            if(cricket == requirement.CricketColor)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
