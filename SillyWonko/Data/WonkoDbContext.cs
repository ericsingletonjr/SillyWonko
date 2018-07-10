using Microsoft.EntityFrameworkCore;
using SillyWonko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Data
{
    public class WonkoDbContext : DbContext
    {
		public WonkoDbContext(DbContextOptions<WonkoDbContext> options) : base(options)
		{

		}

		public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Method that is part of DbContext. We override it to fill in
        /// the needed data that we want to seed in our database. Everytime we
        /// add or update this data, a new migration will be needed to update the
        /// database accordingly.
        /// </summary>
        /// <param name="modelBuilder">This is an object that is used for EF</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new {ID = 1, Name = "product1", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 2, Name = "product2", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 3, Name = "product3", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 4, Name = "product4", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 5, Name = "product5", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 6, Name = "product6", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 7, Name = "product7", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 8, Name = "product8", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 9, Name = "product9", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" },
                new {ID = 10, Name = "product10", Sku = "ABCD0001", Price = 9.99M, Description = "description", Image = "" }
            );
        }

	}
}
