using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var player = await this.accountService.GetPlayerAsync(this.User);

            
            return this.View(player);
        }

        
        public async Task<IActionResult> StatsInfo(int id)
        {
            var stats = await this.statsService.GetStatsByIdAsync(id);

            
            var model = this.mapper.Map<StatsViewModel>(stats);

            return this.PartialView("_StatsPartial", model);
            
        }

        public async Task<IActionResult> ItemInfo(string data)
        {

            var model = JsonConvert.DeserializeObject<ItemInfoViewModel>(data);

            

            return this.PartialView("_ItemInfoPartial", model);

        }
    }
}