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
        public async Task<IActionResult> Train()
        {
            
            

            return this.View();
        }

        [Route(LocationConstants.UniversityQuestPath)]
        public async Task<IActionResult> Quest()
        {



            return this.View();
        }
    }
}