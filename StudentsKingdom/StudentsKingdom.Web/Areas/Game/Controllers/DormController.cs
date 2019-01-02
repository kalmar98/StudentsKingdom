using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using StudentsKingdom.Web.Areas.Game.Models.Dorm;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area("Game")]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class DormController : Controller
    {
        private readonly IStatsService statsService;
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public DormController(IStatsService statsService, IAccountService accountService, IMapper mapper)
        {
            this.statsService = statsService;
            this.accountService = accountService;
            this.mapper = mapper;
        }


        [Route("/Game")]
        public async Task<IActionResult> Dorm()
        {
            var user = await this.accountService.GetUserAsync(this.User);

            
            return this.View(user);
        }

        //[HttpPost]
        public async Task<IActionResult> StatsInfo(int id)
        {
            var stats = await this.statsService.GetStatsByIdAsync(id);

            
            var model = this.mapper.Map<StatsViewModel>(stats);

            return this.PartialView("_StatsPartial", model);
            
        }
    }
}