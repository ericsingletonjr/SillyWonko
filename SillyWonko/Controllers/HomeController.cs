using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SillyWonko.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Action to grab the index of the home controller
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
