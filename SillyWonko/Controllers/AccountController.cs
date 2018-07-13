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
                    Claim nameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    await _userManager.AddClaimAsync(user, nameClaim);
                    //await _userManager.AddToRoleAsync(user, ApplicationRoles.Amdin);
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index","Home");
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
            var result = await _signInManager.PasswordSignInAsync(lvm.Email,
                lvm.Password, false, false);
            if (result.Succeeded)
            {
               bool check =  _signInManager.IsSignedIn(User);

                return RedirectToAction("Index", "Home");
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
