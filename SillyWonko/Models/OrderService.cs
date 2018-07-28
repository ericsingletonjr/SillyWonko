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
    public class OrderService : IOrderService
    {
        private WonkoDbContext _context;

        public OrderService(WonkoDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that allows us to create an order for our records when the
        /// user is at checkout
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <param name="totalPrice">Total price of the order</param>
        /// <returns>Newly created order</returns>
        public async Task<Order> CreateOrder(ApplicationUser user, decimal totalPrice)
        {
            Order newOrder = new Order
            {
                UserID = user.Id,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Today
            };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var order = await _context.Orders.Where(o =>
                                             (o.UserID == user.Id) &&
                                             (o.IsCheckedOut != true))
                                             .OrderBy(o => o.ID)
                                             .FirstOrDefaultAsync();                                            
            return order;
        }
        /// <summary>
        /// Action that allows us to create a record of sold items at 
        /// checkout
        /// </summary>
        /// <param name="order">Order the items are attached to</param>
        /// <param name="cartItem">CartItem being recorded</param>
        /// <returns>StatusCode of Created</returns>
        public async Task<HttpStatusCode> CreateSoldProduct(Order order, CartItem cartItem)
        {
            SoldProduct sold = new SoldProduct
            {
                ProductID = cartItem.ProductID,
                OrderID = order.ID,
                Quantity = cartItem.Quantity
            };

            await _context.SoldProducts.AddAsync(sold);
            await _context.SaveChangesAsync();
            return HttpStatusCode.Created;
        }
        /// <summary>
        /// Action that lets us mark if the order is completed in our
        /// records
        /// </summary>
        /// <param name="id">ID of the specific order</param>
        /// <returns>StatusCode Ok or Badrequest</returns>
        public async Task<HttpStatusCode> OrderComplete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsCheckedOut = true;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        /// <summary>
        /// Action that allows us to remove an order if needed.
        /// Admin purposes
        /// </summary>
        /// <param name="id">id of specific order</param>
        /// <returns>StatusCode ok or badrequest</returns>
        public async Task<HttpStatusCode> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if(order == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var soldProducts = await _context.SoldProducts.Where(i => i.OrderID == id).ToListAsync();
            _context.RemoveRange(soldProducts);
            _context.Remove(order);
            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Action that lets us remove a specific sold item.
        /// Admin purposes
        /// </summary>
        /// <param name="id">id of specific item</param>
        /// <returns>StatusCode of Ok or Badrequest</returns>
        public async Task<HttpStatusCode> DeleteSoldProduct(int id)
        {
            var soldProduct = await _context.SoldProducts.FindAsync(id);
            if (soldProduct == null)
            {
                return HttpStatusCode.BadRequest;
            }

            _context.SoldProducts.Remove(soldProduct);
            await _context.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Action that allows us to view all orders from the
        /// database. Admin purposes
        /// </summary>
        /// <returns>List of Orders</returns>
        public async Task<List<Order>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }
		/// <summary>
		/// Action that allows us to view most recent orders from the
		/// database. Admin purposes
		/// </summary>
		/// <returns>List of Orders</returns>
		public async Task<List<Order>> GetRecentOrders()
		{
			var orders = await _context.Orders.OrderByDescending(o => o.ID).Take(20).ToListAsync();
		
			foreach (var order in orders)
			{
				order.Products = await _context.SoldProducts.Where(p => p.OrderID == order.ID).ToListAsync();
			}
			return orders;
		}
		/// <summary>
		/// Action that lets us get all the associated orders
		/// based on a specific userIds
		/// </summary>
		/// <param name="userID">UserID</param>
		/// <returns>List of Orders associated with the specific user</returns>
		public async Task<List<Order>> GetOrdersByUserID(string userID)
        {
            var orders = await _context.Orders.Where(i => i.UserID == userID).ToListAsync();
            return orders;
        }
        /// <summary>
        /// Action allows us to get an order specifically
        /// by its id
        /// </summary>
        /// <param name="id">ID of order</param>
        /// <returns>Order</returns>
        public async Task<Order> GetOrderByID(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order;
        }
        /// <summary>
        /// Action that lets us get the most recent order
        /// made by the user
        /// </summary>
        /// <param name="userID">UserId</param>
        /// <returns>Recent Order</returns>
        public async Task<Order> GetRecentOrderByUserID(string userID)
        {
            var order = await _context.Orders.Where(i => i.UserID == userID && 
                                            i.IsCheckedOut != true)
                                             .OrderBy(o => o.ID)
                                             .FirstOrDefaultAsync();
            return order;
        }
        /// <summary>
        /// Action that lets us grab all the sold products
        /// associated with a specific order
        /// </summary>
        /// <param name="id">Id of the order</param>
        /// <returns></returns>
        public async Task<List<SoldProduct>> GetSoldProducts(int id)
        {
            var soldProducts = await _context.SoldProducts.Where(i => i.OrderID == id).ToListAsync();
            return soldProducts;
        }
        /// <summary>
        /// Action that lets us update a specific order. Admin purposes
        /// </summary>
        /// <param name="userID">userId</param>
        /// <param name="order">Order to update</param>
        /// <returns>Updated Order</returns>
        public async Task<Order> UpdateOrder(int id, Order order)
        {
            var previousOrder = await _context.Orders.FindAsync(id);
            if (previousOrder != null)
            {
                order.ID = previousOrder.ID;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            return order;
        }
        /// <summary>
        /// Action that lets us update a specific sold product.
        /// Admin purposes
        /// </summary>
        /// <param name="id">id of sold product</param>
        /// <param name="product">updated sold product</param>
        /// <returns>Sold product</returns>
        public async Task<SoldProduct> UpdateSoldProduct(int id, SoldProduct product)
        {
            var previousProduct = await _context.SoldProducts.FindAsync(id);
            if (previousProduct != null)
            {
                product.ID = previousProduct.ID;
                _context.SoldProducts.Update(product);
                await _context.SaveChangesAsync();
                return product;
            }
            return product;
        }
    }
}
