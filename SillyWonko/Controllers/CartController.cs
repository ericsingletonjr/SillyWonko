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

                foreach (CartItem item in cart.CartItems)
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
        /// <summary>
        /// Action that gives the user the ability to finish their checkout
        /// process
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>A view</returns>
        [HttpGet]
        public async Task<IActionResult> Checkout(UserViewModel uvm)
        {
            if (uvm.Products == null)
            {
                var user = await _userManager.GetUserAsync(User);
                Order currentOrder = await _order.GetRecentOrderByUserID(user.Id);
                if (currentOrder == null)
                {
                    return RedirectToAction("Index", "Shop");
                }
                uvm.SoldProducts = await _order.GetSoldProducts(currentOrder.ID);
                List<CartItem> products = new List<CartItem>();
                foreach (SoldProduct product in uvm.SoldProducts)
                {
                    CartItem cartItem = new CartItem
                    {
                        Product = await _context.GetProductByID(product.ProductID),
                        Quantity = product.Quantity
                    };
                    products.Add(cartItem);
                }
                Cart placeholder = new Cart
                {
                    CartItems = products
                };
                uvm.Cart = placeholder;
                uvm.Total = currentOrder.TotalPrice;
                uvm.Order = currentOrder;

                return View(uvm);
            }
            return View(uvm);
        }
        /// <summary>
        /// This action begins the process of beginning our order creation.
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>A View</returns>
        [HttpPost]
        public async Task<IActionResult> CheckOut(UserViewModel uvm)
        {
            var user = await _userManager.GetUserAsync(User);
            Order newOrder = await _order.CreateOrder(user, uvm.Total);
            List<CartItem> cartItems = await _cart.GetCartItems(uvm.Cart.ID);

            foreach (CartItem item in cartItems)
            {
                await _order.CreateSoldProduct(newOrder, item);
            }
            Cart complete = await _cart.GetCart(user.Id);

            complete.IsCheckedOut = true;

            newOrder.Products = await _order.GetSoldProducts(newOrder.ID);
            List<Product> products = new List<Product>();
            foreach (SoldProduct item in newOrder.Products)
            {
                products.Add(await _context.GetProductByID(item.ProductID));
            }

            complete.CartItems = cartItems;
            uvm = new UserViewModel {
                Order = newOrder,
                Products = products,
                Total = uvm.Total,
                Cart = complete
            };

            await _cart.DeleteCart(complete.ID);
            await _cart.CreateCart(user);
            return View(uvm);

        }

        [HttpPost]
        public async Task<IActionResult> Complete(UserViewModel uvm)
        {
            await _order.OrderComplete(uvm.Order.ID);
            return RedirectToAction("Index","Shop");
        }
    }
}
