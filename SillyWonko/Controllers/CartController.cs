using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    public class CartController : Controller
    {
        private IWarehouse _context;
        private ICartService _cart;
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }

        public CartController(IWarehouse context, ICartService cart,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _cart = cart;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var userID = _userManager.GetUserId(User);
            if (!String.IsNullOrEmpty(userID))
            {
                var cart = await _cart.GetCart(userID);
                cart.CartItems = await _cart.GetCartItems(cart.ID);

                foreach(CartItem item in cart.CartItems)
                {
                    item.Product = await _context.GetProductByID(item.ProductID);
                }

                return View(new UserViewModel { Cart = cart });
            }
            return RedirectToAction("Index", "Shop");
        }
    }
}
