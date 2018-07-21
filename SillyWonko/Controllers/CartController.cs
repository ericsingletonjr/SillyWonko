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
        private IOrderService _order;
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }

        public CartController(IWarehouse context, ICartService cart, IOrderService order,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _cart = cart;
            _order = order;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Action that gives the user a view of their cart. It gives them the total price
        /// as well as an option to checkout and complete their order
        /// </summary>
        /// <returns>View with a UserViewModel</returns>
        public async Task<IActionResult> Index()
        {
            decimal total = 0;
            var userID = _userManager.GetUserId(User);
            if (!String.IsNullOrEmpty(userID))
            {
                var cart = await _cart.GetCart(userID);
                cart.CartItems = await _cart.GetCartItems(cart.ID);

                foreach(CartItem item in cart.CartItems)
                {
                    item.Product = await _context.GetProductByID(item.ProductID);
                    total = total + (item.Product.Price * item.Quantity);
                }
                UserViewModel uvm = new UserViewModel
                {
                    Cart = cart,
                    Total = total
                };

                return View(uvm);
            }
            return RedirectToAction("Index", "Shop");
        }

        [HttpGet]
        public IActionResult Checkout(UserViewModel uvm)
        {
            return View(uvm);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(UserViewModel uvm)
        {
            var user = await _userManager.GetUserAsync(User);
            Order newOrder = await _order.CreateOrder(user, uvm.Total);
            List<CartItem> cartItems = await _cart.GetCartItems(uvm.Cart.ID);

            foreach(CartItem item in cartItems)
            {
                await _order.CreateSoldProduct(newOrder, item);
            }
            Cart complete = await _cart.GetCart(user.Id);

            complete.IsCheckedOut = true;
            await _cart.UpdateCart(complete.ID, complete);
            
            newOrder.Products = await _order.GetSoldProducts(newOrder.ID);
            List<Product> products = new List<Product>();
            foreach(SoldProduct item in newOrder.Products)
            {
                products.Add(await _context.GetProductByID(item.ProductID));
            }

            return View(new UserViewModel { Order = newOrder, Products = products });
        }
    }
}
