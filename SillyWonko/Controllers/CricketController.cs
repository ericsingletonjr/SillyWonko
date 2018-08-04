using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Controllers
{
    [Authorize]
    public class CricketController : Controller
    {
        [Authorize(Policy = "Bronze")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Silver")]
        public IActionResult Silver()
        {
            return View();
        }

        [Authorize(Policy = "Golden")]
        public IActionResult Golden()
        {
            return View();
        }
    }
}
