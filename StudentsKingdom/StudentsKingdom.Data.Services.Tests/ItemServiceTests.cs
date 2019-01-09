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
    public class ItemServiceTests
    {
        private ApplicationDbContext context;
        private IInventoryItemService inventoryItemService;
        private IItemService itemService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public ItemServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.itemService = provider.GetService<IItemService>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreateItemShouldCreateItem()
        {
            var expected = new Item
            {
                Id = 1,
                Name = "test",
                Type = ItemType.Weapon,
                Coins = 0,
            };

            await this.itemService.CreateItemAsync("test", 0, ItemType.Weapon, null, null);

            var actual = await this.context.Items.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateItemShouldReturnItem()
        {
            var expected = new Item
            {
                Id = 1,
                Name = "test",
                Type = ItemType.Weapon,
                Coins = 0,
            };

            var actual = await this.itemService.CreateItemAsync("test", 0, ItemType.Weapon, null, null);

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetItemByIdShouldReturnItem()
        {
            var expected = new Item
            {
                Id = 1,
                Name = "test",
                Type = ItemType.Weapon,
                Coins = 0,
            };

            await this.itemService.CreateItemAsync("test", 0, ItemType.Weapon, null, null);

            var actual = await this.itemService.GetItemAsync(1);

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetItemByNameShouldReturnItem()
        {
            var expected = new Item
            {
                Id = 1,
                Name = "test",
                Type = ItemType.Weapon,
                Coins = 0,
            };

            await this.itemService.CreateItemAsync("test", 0, ItemType.Weapon, null, null);

            var actual = await this.itemService.GetItemAsync("test");

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetItemByIdShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.itemService.GetItemAsync(1);
            });

        }

        [Fact]
        public async Task GetItemByNameShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.itemService.GetItemAsync("test");
            });

        }

        [Fact]
        public async Task SeedItemsShouldSeedItems()
        {
            await this.itemService.SeedItemsAsync();

            Assert.True(await this.context.Items.AnyAsync());

        }

        [Fact]
        public async Task SeedItemsShouldSeedFourItems()
        {
            await this.itemService.SeedItemsAsync();

            Assert.Equal(4, await this.context.Items.CountAsync());

        }
    }
}
