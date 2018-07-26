using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SillyWonko.Models.ViewModels
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public LoginViewModel Login { get; set; }
        public ExternalLoginViewModel External { get; set; }
        public RegisterViewModel Register { get; set; }
        public ShippingViewModel Shipping { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
        public CartItem CartItem { get; set; }
        public Cart Cart { get; set; }
        public decimal Total { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<SoldProduct> SoldProducts { get; set; }
    }
}
