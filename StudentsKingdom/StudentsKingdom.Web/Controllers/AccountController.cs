using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants;
using StudentsKingdom.Common.Enums;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        { 
            if (this.ModelState.IsValid)
            {
                var player = await this.accountService.GetPlayerAsync(model.Username, model.Password);

                if (player != null)
                {
                    await this.accountService.LoginAsync(player, model.RememberMe);

                    if(player.UserName == UserRoles.Admin.ToString())
                    {
                        return this.Redirect("/Administration");
                    }

                    return this.Redirect("/Game");
                }

            }

            this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.LoginError;
            return this.RedirectToAction("Index", "Home");

        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var usernameOrEmailTaken = await this.accountService.AreUsernameOrEmailTakenAsync(model.Username, model.Email);

                if (!usernameOrEmailTaken)
                {
                    var player = await this.accountService.RegisterAsync(model.Username, model.Password, model.Email);

                    await this.accountService.LoginAsync(player, false);

                    return this.Redirect("/Game");
                }

            }

            this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.RegisterError;
            return this.View();
        }

        public async Task<IActionResult> Logout()
        {
            await this.accountService.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }
    }
}
