using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class InventoryServiceTests
    {
        private ApplicationDbContext context;
        private IInventoryItemService inventoryItemService;
        private IInventoryService inventoryService;
        private IItemService itemService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public InventoryServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.inventoryService = provider.GetService<IInventoryService>();
            this.itemService = provider.GetService<IItemService>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreateInventoryShouldCreateInventory()
        {
            var expected = new Inventory
            {
                Id = 1,
                Capacity = CharacterConstants.InventoryCapacity
            };

            await this.inventoryService.CreateInventoryAsync();

            var actual = await this.context.Inventories.FirstOrDefaultAsync();
            
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateInventoryForLocationShouldCreateInventoryForLocationWithItems()
        {
            await this.itemService.SeedItemsAsync();

            await this.inventoryService.CreateInventoryAsync(LocationType.Blacksmith);

            var actual = await this.context.Inventories.FirstOrDefaultAsync();

            Assert.True(actual.InventoryItems.Any());
        }

        [Fact]
        public async Task CreateInventoryShouldReturnInventory()
        {
            var expected = new Inventory
            {
                Id = 1,
                Capacity = CharacterConstants.InventoryCapacity
            };

            var actual = await this.inventoryService.CreateInventoryAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateInventoryForLocationShouldReturnInventoryForLocationWithItems()
        {
            await this.itemService.SeedItemsAsync();

            var actual = await this.inventoryService.CreateInventoryAsync(LocationType.Blacksmith);

            Assert.True(actual.InventoryItems.Any());
        }

        [Fact]
        public async Task CreateInventoryForLocationThatDoNotNeedInventoryShouldReturnNull()
        {
            var actual = await this.inventoryService.CreateInventoryAsync(LocationType.Dorm);

            Assert.Null(actual);
        }

        [Fact]
        public async Task IsInventoryFullShouldReturnTrue()
        {
            var inventory = new Inventory
            {
                Capacity = 2
            };

            inventory.InventoryItems.Add(new InventoryItem { IsEquipped = false });
            inventory.InventoryItems.Add(new InventoryItem { IsEquipped = false });

            var actual = await this.inventoryService.IsInventoryFullAsync(inventory);

            Assert.True(actual);
        }

        [Fact]
        public async Task IsInventoryFullShouldReturnFalse()
        {
            var inventory = new Inventory
            {
                Capacity = 2
            };

            inventory.InventoryItems.Add(new InventoryItem { IsEquipped = false });

            var actual = await this.inventoryService.IsInventoryFullAsync(inventory);

            Assert.False(actual);
        }
    }
}
