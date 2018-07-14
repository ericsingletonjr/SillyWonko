using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SillyWonko.Models.Interfaces
{
    public interface IWarehouse
    {
        Task<HttpStatusCode> CreateProduct(Product product);
        Task<Product> GetProductByID(int id);
        Task<List<Product>> GetProducts();
        Task<Product> UpdateProduct(int id, Product product);
        Task<HttpStatusCode> DeleteProduct(int id);
    }
}
