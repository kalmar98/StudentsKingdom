using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class LocationServiceTests
    {
        private ApplicationDbContext context;
        private ILocationService locationService;
        private IInventoryService inventoryService;
        private IInventoryItemService inventoryItemService;
        private IItemService itemService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public LocationServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStatsService, StatsService>();


            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.locationService = provider.GetService<ILocationService>();
            this.inventoryService = provider.GetService<IInventoryService>();
            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.itemService = provider.GetService<IItemService>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreateLocationShouldCreateLocation()
        {
            var expected = new Location
            {
                Id = 1,
                Name = "Dorm"
            };

            await this.locationService.CreateLocationAsync("Dorm", null, LocationType.Dorm);

            var actual = await this.context.Locations.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateLocationShouldReturnLocation()
        {
            var expected = new Location
            {
                Id = 1,
                Name = "Dorm"
            };

            var actual = await this.locationService.CreateLocationAsync("Dorm", null, LocationType.Dorm);

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetLocationShouldReturnLocation()
        {
            var expected = new Location
            {
                Id = 1,
                Name = "Dorm"
            };

            await this.locationService.CreateLocationAsync("Dorm", null, LocationType.Dorm);

            var actual = await this.locationService.GetLocationAsync(LocationType.Dorm);

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetLocationShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.locationService.GetLocationAsync(LocationType.Dorm);
            });

        }

        [Fact]
        public async Task SeedLocationsShouldSeedLocations()
        {
            await this.itemService.SeedItemsAsync();

            await this.locationService.SeedLocationsAsync();

            Assert.True(await this.context.Locations.AnyAsync());

        }

        [Fact]
        public async Task SeedLocationsShouldSeedFiveLocations()
        {
            await this.itemService.SeedItemsAsync();

            await this.locationService.SeedLocationsAsync();

            Assert.Equal(5, await this.context.Locations.CountAsync());

        }
    }
}
