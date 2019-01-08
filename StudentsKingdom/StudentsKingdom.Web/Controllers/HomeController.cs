using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Enums;
using StudentsKingdom.Web.Models;

namespace StudentsKingdom.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(UserRoles.Admin.ToString()))
                {
                    return this.Redirect(LocationConstants.AdministrationPath);
                }

                return this.Redirect(LocationConstants.GamePath);
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
