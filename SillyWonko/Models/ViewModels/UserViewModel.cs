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
        public IEnumerable<Claim> Claims { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public static UserViewModel UserDetails(ApplicationUser user, IEnumerable<Claim> claims)
        {
            UserViewModel uvm = new UserViewModel
            {
                User = user,
                Claims = claims
            };
            return uvm;
        }
    }
}
