using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    public class ShopController : Controller
    {
        private IWarehouse _context;

        public ShopController(IWarehouse context)
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
