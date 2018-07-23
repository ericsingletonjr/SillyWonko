using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SillyWonko.Models.Interfaces
{
    public interface ICartService
    {
		Task<HttpStatusCode> CreateCart(ApplicationUser user);
		Task<HttpStatusCode> CreateCartItem(Cart cart, CartItem cartItem);
		Task<List<CartItem>> GetCartItems(int id);
		Task<CartItem> GetCartItem(int id);
		Task<HttpStatusCode> DeleteCartItem(int id);
		Task<HttpStatusCode> DeleteCart(int id);
		Task<CartItem> UpdateCartItem(int id, CartItem cartItem);
		Task<Cart> UpdateCart(int id, Cart cart);
		Task<List<Cart>> GetAllCarts();
		Task<Cart> GetCart(string id);
    }
}
