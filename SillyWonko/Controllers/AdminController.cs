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
            return View(new UserViewModel());
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(UserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                HttpStatusCode response = await _context.CreateProduct(uvm.Product);
                if (response == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Admin");
                }
                return View(uvm.Product);
            }
            return View(uvm.Product);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            UserViewModel uvm = new UserViewModel();
            if (id.HasValue)
            {
                uvm.Product = await _context.GetProductByID(id.Value);
                if(uvm.Product != null)
                {
                    return View(uvm);
                }
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int? id, [Bind("ID,Name,Sku,Image,Price,Description")]Product product)
        {
            UserViewModel uvm = new UserViewModel();
            uvm.Product = product;
            if (id.HasValue)
            {
                var updated = await _context.UpdateProduct(id.Value, uvm.Product);
                return RedirectToAction("Index", "Admin");
            }
            return View(uvm.Product);
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
