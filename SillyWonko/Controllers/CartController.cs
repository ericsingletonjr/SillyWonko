using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private IEmailSender _emailSender;

        public CartController(IWarehouse context, ICartService cart, IOrderService order,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailSender emailSender)
        {
            _context = context;
            _cart = cart;
            _order = order;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        /// Action that allows users to remove a specific item from their cart
        /// </summary>
        /// <param name="id">Id of the cart item</param>
        /// <returns>Redirect to the cart</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            await _cart.DeleteCartItem(id);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Action that allows users to update the amount of an item in their cart
        /// </summary>
        /// <param name="id">id of cart item</param>
        /// <param name="quantity">new quantity item</param>
        /// <returns>redirect to action</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateItem(int id, int quantity)
        {
            var cartItem = await _cart.GetCartItem(id);
            cartItem.Quantity = quantity;
            var update = await _cart.UpdateCartItem(id, cartItem);
            if (update != null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Action that gives the user the ability to finish their checkout
        /// process
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>A view</returns>
        [HttpGet]
        [Authorize(Policy = "Member")]
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
            uvm = new UserViewModel
            {
                Order = newOrder,
                Products = products,
                Total = uvm.Total,
                Cart = complete
            };

            await _cart.DeleteCart(complete.ID);
            await _cart.CreateCart(user);
            return View(uvm);

        }
        /// <summary>
        /// Action to prevent users from accessing this
        /// page by itself
        /// </summary>
        /// <returns>Redirect to shop</returns>
        [HttpGet]
        public IActionResult Complete()
        {
            return RedirectToAction("Index", "Shop");
        }
        /// <summary>
        /// Action that completes the order and gives a brief order
        /// invoice and then sends an email to the user
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>View with a UserViewModel</returns>
        [HttpPost]
        [Authorize(Policy = "Member")]
        public async Task<IActionResult> Complete(UserViewModel uvm)
        {
            await _order.OrderComplete(uvm.Order.ID);

            var order = await _order.GetOrderByID(uvm.Order.ID);
            order.Products = await _order.GetSoldProducts(uvm.Order.ID);
            List<CartItem> productList = new List<CartItem>();

            foreach (SoldProduct product in order.Products)
            {
                CartItem cartItem = new CartItem
                {
                    Product = await _context.GetProductByID(product.ProductID),
                    Quantity = product.Quantity
                };
                productList.Add(cartItem);
            }
            Cart placeHolder = new Cart();
            var orderPrice = order.TotalPrice;
            var user = await _userManager.GetUserAsync(User);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<p>Thanks for being so curious! Have another look at your silly selections.</p>");

            foreach (var product in productList)
            {
                sb.AppendLine($"<h3>{product.Product.Name}</h3>");
                sb.AppendLine($"<p>Qty: {product.Quantity}</p>");
                sb.AppendLine($"<p>Price: {product.Product.Price}</p>");
            }
            sb.AppendLine($"<h3>Curious Total: {orderPrice}</h3>");

            await _emailSender.SendEmailAsync(user.Email, "Silly Invoice", sb.ToString());

            placeHolder.CartItems = productList;
            uvm.Cart = placeHolder;
            uvm.Order = order;

            return View(uvm);
        }
    }
}