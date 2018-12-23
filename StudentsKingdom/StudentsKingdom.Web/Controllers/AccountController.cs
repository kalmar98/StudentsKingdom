using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult LoginWindow()
        {
            return this.PartialView("_LoginWindowPartial");
        }

        [HttpPost]
        public IActionResult Login()
        {



            return this.RedirectToPage("");
        }
    }
}
