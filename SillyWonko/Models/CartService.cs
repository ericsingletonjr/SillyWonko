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
		public Task<HttpStatusCode> CreateCart(Cart cart)
		{
			throw new NotImplementedException();
		}

		public Task<HttpStatusCode> CreateCartItem(CartItem cartItem)
		{
			throw new NotImplementedException();
		}

		public Task<HttpStatusCode> DeleteCart(int id)
		{
			throw new NotImplementedException();
		}

		public Task<HttpStatusCode> DeleteCartItem(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Cart>> GetAllCarts()
		{
			throw new NotImplementedException();
		}

		public Task<Cart> GetCart(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<CartItem>> GetCartItems()
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
