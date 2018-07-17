using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
	public class Cart
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public List<CartItem> CartItems { get; set; }
		public bool IsCheckedOut { get; set; } = false;
	}
}
