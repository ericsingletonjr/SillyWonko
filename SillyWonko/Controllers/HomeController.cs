using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models.Interfaces;

namespace SillyWonko.Controllers
{
    public class HomeController : Controller
    {
        private IWarehouse _context;

        public HomeController(IWarehouse context)
        {
            _context = context;
        }
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
