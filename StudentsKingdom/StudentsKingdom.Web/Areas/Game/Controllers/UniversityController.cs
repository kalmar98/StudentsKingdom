using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Services.Contracts;
using StudentsKingdom.Web.Areas.Game.Models.University;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area(UserConstants.GameArea)]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class UniversityController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IItemService itemService;
        private readonly IAccountService accountService;
        private readonly IInventoryService inventoryService;
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public UniversityController(ILocationService locationService, IItemService itemService, IAccountService accountService, IInventoryService inventoryService, ICharacterService characterService, IMapper mapper)
        {
            this.locationService = locationService;
            this.itemService = itemService;
            this.accountService = accountService;
            this.inventoryService = inventoryService;
            this.characterService = characterService;
            this.mapper = mapper;
        }

        [Route(LocationConstants.UniversityPath)]
        public async Task<IActionResult> University()
        {
            var location = await this.locationService.GetLocationByTypeAsync(LocationType.University);

            //може да се сложи мапинг later

            return this.View(location);
        }


        [Route(LocationConstants.UniversityTrainPath)]
        public async Task<IActionResult> Train(string stat)
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.TrainAsync(player.Character, stat);

            if (result)
            {
                return this.Redirect(LocationConstants.GamePath);
            }

            return this.Redirect(LocationConstants.UniversityPath);
            
        }

        [Route(LocationConstants.UniversityQuestPath)]
        public async Task<IActionResult> Quest(string data)
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.QuestAsync(player.Character, data);

            if(result != null)
            {
                var model = this.mapper.Map<QuestInfoViewModel>(result);
                return this.PartialView("_QuestResultPartial",model);
            }

            return this.PartialView("_QuestResultPartial", null);
        }


    }
}