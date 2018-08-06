using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SillyWonko.Models;
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Pages
{
    [Authorize]
    public class OrdersModel : PageModel
    {
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
		private IOrderService _order;
		private IWarehouse _context;

		[BindProperty]
		public List<Order> Orders { get; set; } = new List<Order>();
		[BindProperty]
		public List<Product> Products { get; set; } = new List<Product>();

		public OrdersModel(IWarehouse context, IOrderService order, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_context = context;
			_order = order;
			_userManager = userManager;
			_signInManager = signInManager;
		}

        public async Task OnGet()
        {
			var userID = _userManager.GetUserId(User);
			Orders = await _order.GetRecent3Orders(userID);
			Products = await _context.GetProducts();
        }
    }
}