using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    public class ShopController : Controller
    {
        private IWarehouse _context;
        private ICartService _cart;
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }

        public ShopController(IWarehouse context, ICartService cart,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _cart = cart;
            _userManager = userManager;
            _signInManager = signInManager;
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
        /// <summary>
        /// Action to grab the details of a specific product
        /// </summary>
        /// <param name="id">Id of product</param>
        /// <returns>returns UserViewModel</returns>
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
        /// <summary>
        /// Action that allows users to add to their own
        /// basket if signed in
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>To Index</returns>
        [HttpPost]
        public async Task<IActionResult> AddToCart(UserViewModel uvm)
        {
            uvm.CartItem.ProductID = uvm.Product.ID;

            var userID = _userManager.GetUserId(User);
            if (!String.IsNullOrEmpty(userID))
            {
                var cart = await _cart.GetCart(userID);
                if (uvm.CartItem.Quantity < 1) uvm.CartItem.Quantity = 1;

                var result = await _cart.CreateCartItem(cart, uvm.CartItem);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
