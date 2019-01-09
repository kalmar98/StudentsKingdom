using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Common.Enums.Items;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class CharacterServiceTests
    {
        private ApplicationDbContext context;
        private ICharacterService characterService;
        private IInventoryItemService inventoryItemService;
        private IInventoryService inventoryService;
        private IItemService itemService;
        private ILocationService locationService;
        private IQuestService questService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public CharacterServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.characterService = provider.GetService<ICharacterService>();
            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.inventoryService = provider.GetService<IInventoryService>();
            this.itemService = provider.GetService<IItemService>();
            this.locationService = provider.GetService<ILocationService>();
            this.questService = provider.GetService<IQuestService>();
            this.statsService = provider.GetService<IStatsService>();
        }
    }
}
