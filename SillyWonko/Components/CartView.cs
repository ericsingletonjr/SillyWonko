using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SillyWonko.Data;
using SillyWonko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Components
{
    public class CartView : ViewComponent
    {
		private WonkoDbContext _context;
		private UserManager<ApplicationUser> _userManager;

		public CartView(WonkoDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync(string userEmail)
		{
			var user = await _userManager.FindByEmailAsync(userEmail);
			Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == user.Id);
			List<CartItem> items = await _context.CartItems
												 .Where(i => i.CartID == cart.ID)
												 .ToListAsync();
			foreach (var item in items)
			{
				item.Product = await _context.Products
					.FirstOrDefaultAsync(p => p.ID == item.ProductID); 
			}
			return View(items);
		}
    }
}
