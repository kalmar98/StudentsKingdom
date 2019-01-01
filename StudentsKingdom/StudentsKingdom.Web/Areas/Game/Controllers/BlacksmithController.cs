using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Services.Contracts;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area("Game")]
    [Authorize(Roles = UserConstants.RoleUser)]
    public class BlacksmithController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IItemService itemService;
        private readonly IAccountService accountService;
        private readonly IInventoryService inventoryService;
        private readonly IMapper mapper;

        public BlacksmithController(ILocationService locationService, IItemService itemService, IAccountService accountService, IInventoryService inventoryService, IMapper mapper)
        {
            this.locationService = locationService;
            this.itemService = itemService;
            this.accountService = accountService;
            this.inventoryService = inventoryService;
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
                return this.Redirect("/Game/Blacksmith");
            }

            var item = await this.itemService.GetItemByIdAsync(itemId);
            var user = await this.accountService.GetUserAsync(this.User);



            if (await this.inventoryService.IsInventoryFullAsync(user.Character.Inventory))
            {
                //да подам стойност за проверка
                return this.Redirect("/Game/Blacksmith");
            }

            //ако няма пари

            return this.Redirect("/Game");
        }
    }
}