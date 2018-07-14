using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SillyWonko.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize(Policy = "Employee")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
