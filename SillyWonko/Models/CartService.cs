using Microsoft.EntityFrameworkCore;
using SillyWonko.Data;
using SillyWonko.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
	public class CartService : ICartService
	{
		private WonkoDbContext _context;

		public CartService(WonkoDbContext context)
		{
			_context = context;
		}

		public async Task<HttpStatusCode> CreateCart(ApplicationUser user)
		{
			Cart cart = new Cart
			{
				UserID = user.Id
			};
			await _context.Carts.AddAsync(cart);
			await _context.SaveChangesAsync();
			return HttpStatusCode.Created;
		}

		public async Task<HttpStatusCode> CreateCartItem(CartItem cartItem)
		{
			await _context.CartItems.AddAsync(cartItem);
			await _context.SaveChangesAsync();
			return HttpStatusCode.Created;
		}

		public Task<HttpStatusCode> DeleteCart(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<HttpStatusCode> DeleteCartItem(int id)
		{
			var cartItem = await _context.CartItems.FindAsync(id);
			if (cartItem == null)
			{
				return HttpStatusCode.BadRequest;
			}
			_context.CartItems.Remove(cartItem);
			await _context.SaveChangesAsync();
			return HttpStatusCode.OK;
		}

		public Task<List<Cart>> GetAllCarts()
		{
			throw new NotImplementedException();
		}

		public async Task<Cart> GetCart(int id)
		{
			var cart = await _context.Carts.FindAsync(id);
			return cart;
		}

		public Task<List<CartItem>> GetCartItems(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Cart> UpdateCart(int id, Cart cart)
		{
			throw new NotImplementedException();
		}

		public Task<CartItem> UpdateCartItem(int id, CartItem cartItem)
		{
			throw new NotImplementedException();
		}
	}
}
