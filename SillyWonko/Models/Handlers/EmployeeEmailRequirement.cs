using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models.Handlers
{
    public class EmployeeEmailRequirement : IAuthorizationRequirement
    {
        public string EmployeeEmail { get; set; }

        public EmployeeEmailRequirement(string email)
        {
            EmployeeEmail = email;
        }
    }
}
