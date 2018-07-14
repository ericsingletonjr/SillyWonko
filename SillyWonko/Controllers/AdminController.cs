using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            UserViewModel uvm = new UserViewModel();
            var productList = await  _context.GetProducts();
            uvm.Products = productList;
            
            return View(uvm);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                HttpStatusCode response = await _context.CreateProduct(product);
                if (response == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Admin");
                }
                return View(product);
            }
            return View(product);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                Product product = await _context.GetProductByID(id.Value);
                if(product != null)
                {
                    return View(product);
                }
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int? id, [Bind("ID,Name,Sku,Image,Price,Description")]Product product)
        {
            if (id.HasValue)
            {
                var updated = await _context.UpdateProduct(id.Value, product);
                return RedirectToAction("Index", "Admin");
            }
            return View(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _context.DeleteProduct(id);
            if (response == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
