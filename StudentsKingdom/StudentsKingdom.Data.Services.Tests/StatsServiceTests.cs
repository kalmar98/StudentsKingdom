using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class StatsServiceTests
    {
        private ApplicationDbContext context;
        private IStatsService statsService;
        private IServiceProvider provider;

        public StatsServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IStatsService, StatsService>();


            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreateStatsShouldCreateStats()
        {
            var expected = new Stats
            {
                Id = 1,
                Vitality = CharacterConstants.StartingStatValue,
                Strength = CharacterConstants.StartingStatValue,
                Agility = CharacterConstants.StartingStatValue,
                Intellect = CharacterConstants.StartingStatValue
            };

            await this.statsService.CreateStatsAsync(
                vitality: CharacterConstants.StartingStatValue,
                strength: CharacterConstants.StartingStatValue,
                agility: CharacterConstants.StartingStatValue,
                intellect: CharacterConstants.StartingStatValue
                );

            var actual = await this.context.Stats.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
                
        }

        [Fact]
        public async Task CreateStatsShouldReturnStats()
        {
            var expected = new Stats
            {
                Id = 1,
                Vitality = CharacterConstants.StartingStatValue,
                Strength = CharacterConstants.StartingStatValue,
                Agility = CharacterConstants.StartingStatValue,
                Intellect = CharacterConstants.StartingStatValue
            };

            var actual = await this.statsService.CreateStatsAsync(
                vitality: CharacterConstants.StartingStatValue,
                strength: CharacterConstants.StartingStatValue,
                agility: CharacterConstants.StartingStatValue,
                intellect: CharacterConstants.StartingStatValue
                );

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetStatsShouldReturnStats()
        {
            var expected = new Stats
            {
                Id = 1,
            };

            await this.statsService.CreateStatsAsync();

            var actual = await this.statsService.GetStatsAsync(1);

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetStatsShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException > (async () =>
            {
                 await this.statsService.GetStatsAsync(1);
            });

        }
    }
}
