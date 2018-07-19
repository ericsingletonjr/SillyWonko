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
using SillyWonko.Models.Interfaces;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private ICartService _cart;
        /// <summary>
        /// Setting up our user creation system with identity
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ICartService cart)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new UserViewModel());
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
        public async Task<IActionResult> Register(UserViewModel uvm)
        {
            RegisterViewModel rvm = uvm.Register;

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
                    await _cart.CreateCart(user);
                    
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
                        Claim fizzy = new Claim("BuzzyFizz", "Silver Cricket Member");
                        Claims.Add(fizzy);
                    }
                    if (randNum % 3 == 0 || user.Email.ToLower() == "bronze@wonko.com")
                    {
                        Claim buzzy = new Claim("BuzzyFizz", "Bronze Cricket Member");
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

                    return RedirectToAction("Index", "Shop");
                }
            }
            return View(uvm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new UserViewModel());
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
        public async Task<IActionResult> Login(UserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(uvm.Login.Email,
                    uvm.Login.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(uvm.Login.Email);

                    if (await _userManager.IsInRoleAsync(user,ApplicationRoles.Administrator))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Whoops, try again.");
                }
            }
            return View(uvm);
        }
        /// <summary>
        /// Action that makes the first call towards the given provider.
        /// It will link to the appropriate third-party signin
        /// </summary>
        /// <param name="provider">Name of the provider</param>
        /// <returns>a challenge</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallBack), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        /// <summary>
        /// Action that is our callback function. This will
        /// get the email information from the third-party response
        /// and populate a form with an email.
        /// </summary>
        /// <param name="remoteError">If it isn't null, an error has occurred</param>
        /// <returns>Different redirects based on result</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string remoteError = null)
        {
            if(remoteError != null)
            {
                TempData["Error"] = "Seems to be an error from the provider";
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Shop");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ExternalLoginViewModel elvm = new ExternalLoginViewModel
            {
                Email = email
            };
            return View("ExternalLogin", new UserViewModel { External = elvm });
            
        }
        /// <summary>
        /// Action that allow us to create a user from the given third-party information.
        /// We can create claims, membership and a cart that will associate with
        /// this new user account.
        /// </summary>
        /// <param name="uvm">UserViewModel</param>
        /// <returns>Redirect to views based on result</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(UserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["Error"] = "Seems to be an issue";
                }

                var user = new ApplicationUser {
                    UserName = uvm.External.Email,
                    Email = uvm.External.Email,
                    FirstName = uvm.User.FirstName,
                    LastName = uvm.User.LastName
                };

                await _cart.CreateCart(user);

                List<Claim> Claims = new List<Claim>();
                Random random = new Random();
                int randNum = random.Next(1, 16);

                if (randNum % 15 == 0)
                {
                    Claim buzzyFizz = new Claim("BuzzyFizz", "Golden Cricket Member");
                    Claims.Add(buzzyFizz);
                }
                if (randNum % 5 == 0)
                {
                    Claim fizzy = new Claim("BuzzyFizz", "Silver Cricket Member");
                    Claims.Add(fizzy);
                }
                if (randNum % 3 == 0)
                {
                    Claim buzzy = new Claim("BuzzyFizz", "Bronze Cricket Member");
                    Claims.Add(buzzy);
                }

                Claim nameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);

                Claims.Add(nameClaim);
                Claims.Add(emailClaim);


                var result = await _userManager.CreateAsync(user);
                await _userManager.AddClaimsAsync(user, Claims);
                await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Shop");
                    }
                }
            }
            return RedirectToAction(nameof(ExternalLoginCallBack), uvm);
        }

        [Route("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["LogOut"] = "Logged Out";
            return RedirectToAction("Index", "Shop");
        }
    }
}
