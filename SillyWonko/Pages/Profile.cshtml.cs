using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SillyWonko.Models;

namespace SillyWonko.Pages.Profile
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string CricketMember { get; set; }

        public ProfileModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;


            Claim member = User.Claims.Where(c => c.Type == "BuzzyFizz").FirstOrDefault();
            if (member != null) CricketMember = member.Value;
            else CricketMember = "mysterious";
        }
    }
}