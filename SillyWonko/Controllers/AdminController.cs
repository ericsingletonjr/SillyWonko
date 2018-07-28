using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private IWarehouse _context;
		private IOrderService _orders;
		private UserManager<ApplicationUser> _userManager { get; set; }
		private SignInManager<ApplicationUser> _signInManager {	get; set; }

		public AdminController(IWarehouse context, IOrderService orders, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
			_orders = orders;
			_userManager = userManager;
			_signInManager = signInManager;
        }
        /// <summary>
        /// Action that gives us the index of the admin dashboard
        /// </summary>
        /// <returns>View with a UserViewModel</returns>
        public async Task<IActionResult> Index()
        {
            UserViewModel uvm = new UserViewModel();
            var productList = await _context.GetProducts();
            uvm.Products = productList;
			var orderList = await _orders.GetRecentOrders();
			uvm.Orders = orderList;
			uvm.Users = new List<ApplicationUser>();
			foreach (var order in orderList)
			{
				ApplicationUser user = new ApplicationUser();
				user.Id = order.UserID;
                var claims = await _userManager.GetClaimsAsync(user);
                string[] fullName = claims.First(c => c.Type == "FullName")
                                        .Value
                                        .Split(" ");
                user.FirstName = fullName[0];
                user.LastName = fullName[1];
				uvm.Users.Add(user);
			}
			return View(uvm);
        }
        /// <summary>
        /// Action that gets us the create view
        /// </summary>
        /// <returns>View with UserViewModel</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserViewModel());
        }
        /// <summary>
        /// Action that allows an admin to create a new
        /// product and add it to the database
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>UserViewModel with the product</returns>
        [HttpPost]
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
            else
            {
                ModelState.AddModelError(string.Empty, "Whoops, try again.");
            }
            return View(uvm.Product);
        }
        /// <summary>
        /// Action that lets the admin see a detailed view of a
        /// product
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>View with a USerViewModel</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            UserViewModel uvm = new UserViewModel();
            if (id.HasValue)
            {
                uvm.Product = await _context.GetProductByID(id.Value);
                if (uvm.Product != null)
                {
                    return View(uvm);
                }
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }
        /// <summary>
        /// Action that lets an admin update the details of a 
        /// specific product in the database
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="product">properties of the product</param>
        /// <returns>Redirects to the dashboard if successful</returns>
        [HttpPost]
        public async Task<IActionResult> Update(int? id, [Bind("ID,Name,Sku,Image,Price,Description")]Product product)
        {
            UserViewModel uvm = new UserViewModel();
            uvm.Product = product;
            if (id.HasValue)
            {
                if (ModelState.IsValid)
                {
                    var updated = await _context.UpdateProduct(id.Value, uvm.Product);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Whoops, try again.");
                }
            }
            return RedirectToAction("Details", new { id = id.Value });
        }
        /// <summary>
        /// Action that allows an admin to remove a product from the
        /// database
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>Redirect to the admin dashboard</returns>
        [HttpPost]
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
