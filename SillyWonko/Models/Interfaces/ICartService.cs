using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SillyWonko.Models.Interfaces
{
    interface ICartService
    {
		Task<HttpStatusCode> CreateCart(Cart cart);
		Task<HttpStatusCode> CreateCartItem(CartItem cartItem);
		Task<List<CartItem>> GetCartItems();
		Task<HttpStatusCode> DeleteCartItem(int id);
		Task<HttpStatusCode> DeleteCart(int id);
		Task<CartItem> UpdateCartItem(int id, CartItem cartItem);
		Task<Cart> UpdateCart(int id, Cart cart);
		Task<List<Cart>> GetAllCarts();
		Task<Cart> GetCart(int id);
    }
}
