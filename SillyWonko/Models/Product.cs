using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
    public class Product
    {
		public int ID { get; set; }
        [Required]
		public string Sku { get; set; }
        [Required]
		public string Name { get; set; }
        [Required]
		public decimal Price { get; set; }
		public string Description { get; set; }
        [Required]
		public string Image { get; set; }
	}
}
