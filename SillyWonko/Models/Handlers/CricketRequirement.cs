using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models.Handlers
{
    public class CricketRequirement : IAuthorizationRequirement
    {
        public string CricketColor { get; set; }

        public CricketRequirement(string cricketColor)
        {
            CricketColor = cricketColor;
        }
    }
}
