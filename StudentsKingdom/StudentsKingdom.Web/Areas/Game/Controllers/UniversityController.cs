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
        private readonly IAccountService accountService;
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public UniversityController(ILocationService locationService, IAccountService accountService, ICharacterService characterService, IMapper mapper)
        {
            this.locationService = locationService;
            this.accountService = accountService;
            this.characterService = characterService;
            this.mapper = mapper;
        }

        [Route(LocationConstants.UniversityPath)]
        public async Task<IActionResult> University()
        {
            return await Task.Run(() =>
            {
                return this.View();
            });
        }

        [HttpPost]
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

        [HttpPost]
        [Route(LocationConstants.UniversityQuestPath)]
        public async Task<IActionResult> Quest(string data)
        {
            var player = await this.accountService.GetPlayerAsync(this.User);

            var result = await this.characterService.QuestAsync(player.Character, data);

            if (result != null)
            {
                var model = this.mapper.Map<QuestInfoViewModel>(result);
                return this.PartialView(LocationConstants.UniversityQuestResultPartialPath, model);
            }

            return this.PartialView(LocationConstants.UniversityQuestResultPartialPath, null);
        }


    }
}