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
        /// <summary>
        /// Action that allows for the creation of a new cart. This cart
        /// is attached given a userID so it knows what user it belongs to
        /// </summary>
        /// <param name="user">Passes in a user to get an id</param>
        /// <returns>StatusCode of Created</returns>
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
        /// <summary>
        /// Action that allows for the creation of a new cart item.
        /// A cart item is passed through and given a cart id to associate with
        /// </summary>
        /// <param name="cartItem">CartItem object</param>
        /// <param name="cart">Cart object</param>
        /// <returns>StatusCode of created</returns>
		public async Task<HttpStatusCode> CreateCartItem(Cart cart, CartItem cartItem)
		{
            cartItem.CartID = cart.ID;

			await _context.CartItems.AddAsync(cartItem);
			await _context.SaveChangesAsync();
			return HttpStatusCode.Created;
		}
        /// <summary>
        /// Action that lets us remove a cart and
        /// remove all associated cartItems with that cart
        /// </summary>
        /// <param name="id">Id of the car</param>
        /// <returns>StatusCode of badrequest or ok depending if the object is null</returns>
        public async Task<HttpStatusCode> DeleteCart(int id)
		{
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var cartItems = await _context.CartItems.Where(i => i.CartID == id).ToListAsync();
            _context.CartItems.RemoveRange(cartItems);

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Action that allows us to remove a specific cart item
        /// </summary>
        /// <param name="id">id of cart Item</param>
        /// <returns>StatusCode of badrequest or ok depending if the object is null</returns>
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
        /// <summary>
        /// Action to grab a list of all carts,
        /// for admin purposes
        /// </summary>
        /// <returns>List of carts</returns>
		public async Task<List<Cart>> GetAllCarts()
		{
            var carts = await _context.Carts.ToListAsync();
            return carts;
		}
        /// <summary>
        /// Action that allows us to get a cart based off of a specific id
        /// of the user
        /// </summary>
        /// <param name="id">id of the current logged in user</param>
        /// <returnsThe found cart</returns>
		public async Task<Cart> GetCart(string id)
		{
            var cart = await _context.Carts.FirstOrDefaultAsync(i => i.UserID == id);
			return cart;
		}
        /// <summary>
        /// Action allows us to grab cart items based off of a given id 
        /// from a cart
        /// </summary>
        /// <param name="id">id of a cart</param>
        /// <returns>A list of cart items</returns>
		public async Task<List<CartItem>> GetCartItems(int id)
		{
            var cartItems = await _context.CartItems.Where(i => i.CartID == id).ToListAsync();
            return cartItems;
		}
        /// <summary>
        /// Action to update a specific cart for admin purposes
        /// </summary>
        /// <param name="id">Id of specific cart</param>
        /// <param name="cart">New cart information</param>
        /// <returns>Updated Cart</returns>
		public async Task<Cart> UpdateCart(int id, Cart cart)
		{
            var previousCart = await _context.Carts.FindAsync(id);
            if(previousCart != null)
            {
                cart.ID = previousCart.ID;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                return cart;
            }
            return cart;
		}
        /// <summary>
        /// Action that allows us to take in a cart item
        /// and update it in the database
        /// </summary>
        /// <param name="id">id of current cart item</param>
        /// <param name="cartItem">CartItem object</param>
        /// <returns>updated cartItem</returns>
		public async Task<CartItem> UpdateCartItem(int id, CartItem cartItem)
		{
            cartItem.ID = id;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
		}
	}
}
