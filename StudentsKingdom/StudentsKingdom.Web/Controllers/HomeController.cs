using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Web.Models;

namespace StudentsKingdom.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Game");
            }

            if(!this.TempData.ContainsKey("LoginError"))
            {
                this.TempData["LoginError"] = "No";
            }
            
            return this.View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
