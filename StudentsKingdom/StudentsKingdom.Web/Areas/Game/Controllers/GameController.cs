using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area("Game")]
    public class GameController : Controller
    {
        [Route("Game")]
        public IActionResult Dorm()
        {
            return View();
        }

        [Route("Game/Canteen")]
        public IActionResult Canteen()
        {
            return View();
        }

        [Route("Game/Blacksmith")]
        public IActionResult Blacksmith()
        {
            return View();
        }

        [Route("Game/Tavern")]
        public IActionResult Tavern()
        {
            return View();
        }

        [Route("Game/University")]
        public IActionResult University()
        {
            return View();
        }

    }
}