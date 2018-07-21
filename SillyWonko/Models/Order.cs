using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
    public class Order
    {
        public int ID { get; set; }
        public List<SoldProduct> Products { get; set; }
        public string UserID { get; set; }
    }
}
