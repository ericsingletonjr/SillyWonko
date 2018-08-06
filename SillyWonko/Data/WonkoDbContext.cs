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
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }

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
                new {ID = 1, Name = "The Lavaloon", Sku = "ABCD0001", Price = 1.99M,
					Description = 
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_one.jpg" },
                new {ID = 2, Name = "Creambeams", Sku = "ABCD0002", Price = 3.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_two.jpg" },
                new {ID = 3, Name = "The Besider", Sku = "ABCD0003", Price = 6.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_three.jpg" },
                new {ID = 4, Name = "Cracklebricks", Sku = "ABCD0004", Price = 2.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_four.jpg" },
                new {ID = 5, Name = "Nugglewhips", Sku = "ABCD0005", Price = 3.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_five.jpg" },
                new {ID = 6, Name = "Dimlylits", Sku = "ABCD0006", Price = 3.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_six.jpg" },
                new {ID = 7, Name = "The Shamewrecker", Sku = "ABCD0007", Price = 4.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_seven.jpg" },
                new {ID = 8, Name = "Blowbuds", Sku = "ABCD0008", Price = 1.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_eight.jpg" },
                new {ID = 9, Name = "Longswallows", Sku = "ABCD0009", Price = 2.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_nine.jpg" },
                new {ID = 10, Name = "Chocrocks", Sku = "ABCD0010", Price = 4.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_ten.jpg" },
                new {ID = 11, Name = "Butterbars", Sku = "ABCD0011", Price = 8.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_eleven.jpg" },
                new {ID = 12, Name = "Junk", Sku = "ABCD0012", Price = 2.99M,
					Description =
						"Too hot be held without drinking gloves, but refreshingly frosty to taste, " +
						"this tropical shapeshifter goes down smooth. Made with real fizzberries.",
					Image = "images/candy_twelve.jpg" }
            );
        }
	}
}
