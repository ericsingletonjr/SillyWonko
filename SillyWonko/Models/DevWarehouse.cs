using Microsoft.AspNetCore.Mvc;
using SillyWonko.Data;
using SillyWonko.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
    public class DevWarehouse : IWarehouse
    {
        private WonkoDbContext _context;

        public DevWarehouse(WonkoDbContext context)
        {
            _context = context;
        }

        public Task<IActionResult> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProductByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateProduct(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
