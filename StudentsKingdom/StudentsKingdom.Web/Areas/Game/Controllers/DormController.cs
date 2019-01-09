using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using StudentsKingdom.Web.Areas.Game.Models.Dorm;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area(UserConstants.GameArea)]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class DormController : Controller
    {
        private readonly IStatsService statsService;
        private readonly IAccountService accountService;
        private readonly IItemService itemService;
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public DormController(IStatsService statsService, IAccountService accountService, IItemService itemService, ICharacterService characterService, IMapper mapper)
        {
            this.statsService = statsService;
            this.accountService = accountService;
            this.itemService = itemService;
            this.characterService = characterService;
            this.mapper = mapper;
        }


        [Route(LocationConstants.GamePath)]
        public async Task<IActionResult> Dorm()
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var model = this.mapper.Map<CharacterViewModel>(player.Character);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StatsInfo(int id)
        {

            var stats = await this.statsService.GetStatsAsync(id);

            if (stats == null)
            {
                return this.NotFound();
            }

            var model = this.mapper.Map<StatsViewModel>(stats);

            return this.PartialView(LocationConstants.StatsPartialPath, model);

        }

        [HttpPost]
        public async Task<IActionResult> ItemInfo(string data)
        {
            return await Task.Run(() =>
            {
                var model = JsonConvert.DeserializeObject<ItemInfoViewModel>(data);

                return this.PartialView(LocationConstants.ItemInfoPartialPath, model);
            });

        }

        [HttpPost]
        public async Task<IActionResult> Equip(int id)
        {
            var item = await this.itemService.GetItemAsync(id);
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.EquipAsync(player.Character, item);

            if (result != null)
            {
                return new JsonResult(item.Type.ToString());
            }

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<IActionResult> Unequip(string data)
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.UnequipAsync(player.Character, data);

            if (result != null)
            {
                return new ObjectResult(result.ToString());
            }

            return new EmptyResult();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await this.itemService.GetItemAsync(id);
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.RemoveAsync(player.Character, item);

            if (result)
            {
                return new JsonResult(item.Type.ToString());
            }

            return new EmptyResult();
        }
    }
}