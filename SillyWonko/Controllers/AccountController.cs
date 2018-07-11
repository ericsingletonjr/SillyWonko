﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SillyWonko.Models;
using SillyWonko.Models.ViewModels;

namespace SillyWonko.Controllers
{
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
        public IActionResult Register() => View(new RegisterViewModel());
        /// <summary>
        /// This action is our POST that takes the information from the Register
        /// View and uses a ViewModel to create a new ApplicationUser. From there,
        /// the user is passed into the userManager context to create the user
        /// </summary>
        /// <param name="rvm">RegisterViewModel</param>
        /// <returns>Redirect to home if successful or register if model is invalid</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            var user = new ApplicationUser
            {
                UserName = rvm.Email,
                Email = rvm.Email,
                FirstName = rvm.FirstName,
                LastName = rvm.LastName,

            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(rvm);
        }
    }
}
