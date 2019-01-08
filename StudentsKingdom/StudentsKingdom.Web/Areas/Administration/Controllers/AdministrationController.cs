using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Constants.User;

namespace StudentsKingdom.Web.Areas.Administration.Controllers
{
    [Area(UserConstants.AdministrationArea)]
    [Authorize(Roles = UserConstants.RoleAdmin)]
    public class AdministrationController : Controller
    {
        [Route(LocationConstants.AdministrationPath)]
        public  IActionResult Administration()
        {
            return View();
        }
    }
}