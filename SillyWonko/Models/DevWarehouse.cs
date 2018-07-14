using Microsoft.AspNetCore.Mvc;
using SillyWonko.Data;
using SillyWonko.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace SillyWonko.Models
{
    public class DevWarehouse : IWarehouse
    {
        private WonkoDbContext _context;

        public DevWarehouse(WonkoDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that allows us to talk to our database
        /// and create a new product
        /// </summary>
        /// <param name="product">product being passed in</param>
        /// <returns>A statuscode of created</returns>
        public async Task<HttpStatusCode> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return HttpStatusCode.Created;
        }
        /// <summary>
        /// Action that allows us to delete a product
        /// from our database
        /// </summary>
        /// <param name="id">id of the product</param>
        /// <returns>A status code of either OK or BadRequest</returns>
        public async Task<HttpStatusCode> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return HttpStatusCode.BadRequest;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Action that lets us view a product by its
        /// specific ID
        /// </summary>
        /// <param name="id">ID of product</param>
        /// <returns>A product or null if bad request</returns>
        public async Task<Product> GetProductByID(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                return product;
            }
            return null;
        }
        /// <summary>
        /// Action that gets a list of all available products
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<List<Product>> GetProducts()

        {
            var products = await _context.Products.ToListAsync();
            return products;
        }
        /// <summary>
        /// Action that allows us to update a specific product
        /// </summary>
        /// <param name="id">id of product</param>
        /// <param name="product">Product we are updating</param>
        /// <returns>Updated product</returns>
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            product.ID = id;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
