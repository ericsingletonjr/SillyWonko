using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
    public class RoleDataInitializer
    {
        private static readonly List<IdentityRole> Roles = new List<IdentityRole>()
        {
            new IdentityRole
            {
                Name = ApplicationRoles.Administrator,
                NormalizedName = ApplicationRoles.Administrator.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        };

    }
}
