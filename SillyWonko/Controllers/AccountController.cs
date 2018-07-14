using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SillyWonko.Models;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        /// <summary>
        /// Setting up our user creation system with identity
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// This action is our POST that takes the information from the Register
        /// View and uses a ViewModel to create a new ApplicationUser. From there,
        /// the user is passed into the userManager context to create the user
        /// </summary>
        /// <param name="rvm">RegisterViewModel</param>
        /// <returns>Redirect to home if successful or register if model is invalid</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("/register")]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = rvm.Email,
                    Email = rvm.Email,
                    FirstName = rvm.FirstName,
                    LastName = rvm.LastName
                };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    List<Claim> Claims = new List<Claim>();
                    Random random = new Random();
                    int randNum = random.Next(1, 16);

                    if (user.FirstName.ToLower() == ApplicationRoles.Administrator.ToLower())
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Administrator);
                        randNum = 15;
                    }

                    if (randNum % 15 == 0 || user.Email.ToLower() == "gold@wonko.com")
                    {
                        Claim buzzyFizz = new Claim("BuzzyFizz", "Golden Cricket Member");
                        Claims.Add(buzzyFizz);
                    }
                    if (randNum % 5 == 0 || user.Email.ToLower() == "silver@wonko.com")
                    {
                        Claim fizzy = new Claim("Fizzy", "Silver Cricket Member");
                        Claims.Add(fizzy);
                    }
                    if (randNum % 3 == 0 || user.Email.ToLower() == "bronze@wonko.com")
                    {
                        Claim buzzy = new Claim("Buzzy", "Bronze Cricket Member");
                        Claims.Add(buzzy);
                    }

                    if(user.Email.ToLower().Split("@")[1] == "wonko.com")
                    {
                        Claim workerClaim = new Claim("Employee", user.Email.ToLower());
                        Claims.Add(workerClaim);
                    }

                    Claim nameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);

                    Claims.Add(nameClaim);
                    Claims.Add(emailClaim);

                    await _userManager.AddClaimsAsync(user, Claims);
                    await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(rvm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Action that takes in a LoginViewModel and is used to check if the database
        /// has the correct information. If not, it will redirect to the login page again
        /// or to the home page if successful
        /// </summary>
        /// <param name="lvm"LoginVieModel></param>
        /// <returns>Redirect or a LoginView</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email,
                    lvm.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(lvm.Email);

                    if (await _userManager.IsInRoleAsync(user,ApplicationRoles.Administrator))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(lvm);
        }

        [Route("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["LogOut"] = "Logged Out";
            return RedirectToAction("Index", "Home");
        }
    }
}
