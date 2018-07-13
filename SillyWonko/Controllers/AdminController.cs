using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;

namespace SillyWonko.Controllers
{
    public class AdminController : Controller
    {
        private IWarehouse _context;

        public AdminController(IWarehouse context)
        {
            _context = context;
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
