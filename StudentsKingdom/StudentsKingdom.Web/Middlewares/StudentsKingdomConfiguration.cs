using Microsoft.AspNetCore.Http;
using StudentsKingdom.Data;
using StudentsKingdom.Data.Services;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Middlewares
{
    public class StudentsKingdomConfiguration
    {
        private readonly RequestDelegate next;

        public StudentsKingdomConfiguration(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountService accountService, IItemService itemService, ILocationService locationService, IEnemyService enemyService, IQuestService questService)
        {
            await accountService.SeedRolesAsync();
            await accountService.SeedAdminAsync();
            await itemService.SeedItemsAsync();
            await locationService.SeedLocationsAsync();
            await enemyService.SeedEnemiesAsync();
            await questService.SeedQuestsAsync();
            await this.next(context);
        }

    }
}
