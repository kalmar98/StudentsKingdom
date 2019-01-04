using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Services.Contracts;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area("Game")]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class BlacksmithController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IItemService itemService;
        private readonly IAccountService accountService;
        private readonly IInventoryService inventoryService;
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public BlacksmithController(ILocationService locationService, IItemService itemService, IAccountService accountService, IInventoryService inventoryService, ICharacterService characterService, IMapper mapper)
        {
            this.locationService = locationService;
            this.itemService = itemService;
            this.accountService = accountService;
            this.inventoryService = inventoryService;
            this.characterService = characterService;
            this.mapper = mapper;
        }

        [Route("/Game/Blacksmith")]
        public async Task<IActionResult> Blacksmith()
        {
            var location = await this.locationService.GetLocationByTypeAsync(LocationType.Blacksmith);

            //може да се сложи мапинг later

            return this.View(location);
        }

        [HttpPost]
        [Route("/Game/Buy")]
        public async Task<IActionResult> Buy(int itemId)
        {
            if (itemId == 0)
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.ChooseItem;
                return this.Redirect("/Game/Blacksmith");
            }

            var item = await this.itemService.GetItemByIdAsync(itemId);
            var player = await this.accountService.GetPlayerAsync(this.User);



            if (await this.inventoryService.IsInventoryFullAsync(player.Character.Inventory))
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.FullInventory;
                return this.Redirect("/Game/Blacksmith");
            }

            if (!await this.characterService.CanAffordAsync(player.Character.Coins, item))
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.CannotAfford;
                return this.Redirect("/Game/Blacksmith");
            }

            await this.characterService.BuyAsync(player.Character, item);
            //тук не знам дали се добавя навсякъде?????
            return this.Redirect("/Game");
        }
    }
}