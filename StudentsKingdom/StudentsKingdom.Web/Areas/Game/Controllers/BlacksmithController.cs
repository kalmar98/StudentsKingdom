using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Services.Contracts;
using StudentsKingdom.Web.Areas.Game.Models.Shared;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area(UserConstants.GameArea)]
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

        [Route(LocationConstants.BlacksmithPath)]
        public async Task<IActionResult> Blacksmith()
        {
            var location = await this.locationService.GetLocationByTypeAsync(LocationType.Blacksmith);

            var model = this.mapper.Map<TradeViewModel>(location);

            return this.View(model);
        }

        [HttpPost]
        [Route(LocationConstants.BlacksmithBuyPath)]
        public async Task<IActionResult> Buy(int itemId)
        {
            if (itemId == 0)
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.ChooseItem;
                return this.Redirect(LocationConstants.BlacksmithPath);
            }

            var item = await this.itemService.GetItemAsync(itemId);
            var player = await this.accountService.GetPlayerAsync(this.User);

            var location = await this.locationService.GetLocationByTypeAsync(LocationType.Blacksmith);

            if(!location.Inventory.InventoryItems.Any(x=>x.Item.Id == item.Id))
            {
                //security
                return this.Redirect(LocationConstants.BlacksmithPath);
            }

            if (await this.inventoryService.IsInventoryFullAsync(player.Character.Inventory))
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.FullInventory;
                return this.Redirect(LocationConstants.BlacksmithPath);
            }

            if (!await this.characterService.CanAffordAsync(player.Character.Coins, item))
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.CannotAfford;
                return this.Redirect(LocationConstants.BlacksmithPath);
            }

            if (await this.characterService.ItemAlreadyBoughtAsync(player.Character, item))
            {
                this.TempData[ExceptionMessages.ViewDataErrorKey] = ExceptionMessages.ItemAlreadyBought;
                return this.Redirect(LocationConstants.BlacksmithPath);
            }

            await this.characterService.BuyAsync(player.Character, item);
            
            return this.Redirect(LocationConstants.GamePath);
        }
    }
}