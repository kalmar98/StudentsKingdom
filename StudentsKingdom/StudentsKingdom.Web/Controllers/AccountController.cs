using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Data.Services.Contracts;
using StudentsKingdom.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult LoginWindow()
        {
            return this.PartialView("_LoginWindowPartial");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        { 
            if (this.ModelState.IsValid)
            {
                var user = this.accountService.GetUserByNameAndPassword(model.Username, model.Password);

                if (user != null)
                {

                    return this.RedirectToPage("");
                }


                
            }

            this.TempData["LoginError"] = "Yes";
            return this.RedirectToAction("Index", "Home");

        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            return this.View();
        }
    }
}
