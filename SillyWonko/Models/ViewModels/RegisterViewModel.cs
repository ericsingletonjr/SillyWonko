using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, StringLength(100), MinLength(3)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "not right")]
        public string ConfirmPassword { get; set; }

    }
}
