using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Company.Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Company.Project.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Swagger()
        {
            return Redirect("/swagger");
        }
    }
}
