using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models
{
    public class Order
    {
        public int ID { get; set; }
        public List<SoldProduct> Products { get; set; }
        public string UserID { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
        [Column(TypeName="Date")]
		[DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
    }
}
