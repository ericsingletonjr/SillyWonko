using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SillyWonko.Models.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(ApplicationUser user, decimal totalPrice);
        Task<HttpStatusCode> CreateSoldProduct(Order order, CartItem cartItem);
        Task<HttpStatusCode> OrderComplete(int id);
        Task<HttpStatusCode> DeleteOrder(int id);
        Task<HttpStatusCode> DeleteSoldProduct(int id);

        Task<List<Order>> GetOrders();
		Task<List<Order>> GetRecentOrders();
		Task<List<Order>> GetRecent3Orders(string userID);
		Task<List<Order>> GetOrdersByUserID(string userID);
        Task<Order> GetRecentOrderByUserID(string userID);
        Task<Order> GetOrderByID(int id);
        Task<List<SoldProduct>> GetSoldProducts(int id);

        Task<Order> UpdateOrder(int id, Order order);
        Task<SoldProduct> UpdateSoldProduct(int id, SoldProduct product);
    }
}
