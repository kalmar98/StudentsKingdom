using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class InventoryItemServiceTests
    {
        private ApplicationDbContext context;
        private IInventoryItemService inventoryItemService;
        private IServiceProvider provider;

        public InventoryItemServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IInventoryItemService, InventoryItemService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();
            this.inventoryItemService = provider.GetService<IInventoryItemService>();

        }

        [Fact]
        public async Task CreateInventoryItemShouldCreateInventoryItem()
        {
            var expected = new InventoryItem();

            await this.inventoryItemService.CreateInventoryItemAsync(new Inventory(), new Item());

            var actual = await this.context.InventoryItems.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateInventoryItemShouldReturnInventoryItem()
        {
            var expected = new InventoryItem();

            var actual = await this.inventoryItemService.CreateInventoryItemAsync(new Inventory(), new Item());

            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
