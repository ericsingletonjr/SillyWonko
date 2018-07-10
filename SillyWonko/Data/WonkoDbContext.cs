using Microsoft.EntityFrameworkCore;
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
	}
}
