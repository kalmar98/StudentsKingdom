using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Item;
using StudentsKingdom.Common.Constants.Location;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Services.Contracts;

namespace StudentsKingdom.Web.Areas.Game.Controllers
{
    [Area(UserConstants.GameArea)]
    [Authorize(Roles = UserConstants.RolePlayer)]
    public class TavernController : Controller
    {

        private readonly ILocationService locationService;
        private readonly IAccountService accountService;
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public TavernController(ILocationService locationService, IAccountService accountService, ICharacterService characterService, IMapper mapper)
        {
            this.locationService = locationService;
            this.accountService = accountService;
            this.characterService = characterService;
            this.mapper = mapper;
        }

        [Route(LocationConstants.TavernPath)]
        public async Task<IActionResult> Tavern()
        {
            return await Task.Run(() =>
            {
                return this.View();
            });
            
        }

        [HttpPost]
        [Route(LocationConstants.TavernPvpPath)]
        public async Task<IActionResult> Pvp()
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var coinsReward = player.Character.Inventory.InventoryItems.Any(x => x.Item.Name == ItemConstants.DefaultRelicName);

            var result = await this.characterService.PvpAsync(player.Character);

            if (result != null)
            {
                var model = coinsReward ? $"{CharacterConstants.PvpWinCoins} Coins" : ItemConstants.DefaultRelicName;
                
                return this.PartialView(LocationConstants.TavernPvpResultPartialPath, model);
            }

            return this.PartialView(LocationConstants.TavernPvpResultPartialPath, null);
        }
    }

    
}