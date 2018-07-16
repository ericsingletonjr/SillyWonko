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
        /// Action to grab the index of the shop controller
        /// </summary>
        /// <returns>View</returns>
        public async Task<IActionResult> Index()
        {
            UserViewModel uvm = new UserViewModel
            {
                Products = await _context.GetProducts()
            };
            
            return View(uvm);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            UserViewModel uvm = new UserViewModel();
            if (id.HasValue)
            {
                uvm.Product = await _context.GetProductByID(id.Value);
                return View(uvm);
            }
            return Redirect("Index");
        }
    }
}
