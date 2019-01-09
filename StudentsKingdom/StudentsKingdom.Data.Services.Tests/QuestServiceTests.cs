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
    public class QuestServiceTests
    {
        private ApplicationDbContext context;
        private IQuestService questService;
        private IEnemyService enemyService;
        private IServiceProvider provider;

        public QuestServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IEnemyService, EnemyService>();
            services.AddScoped<IStatsService, StatsService>();


            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();
            this.questService = provider.GetService<IQuestService>();
            this.enemyService = provider.GetService<IEnemyService>();
        }

        [Fact]
        public async Task CreateQuestShouldCreateQuest()
        {
            var expected = new Quest
            {
                Id = 1,
                Name = "test",
                Coins = 420,
                Enemy = null,
            };

            await this.questService.CreateQuestAsync("test", 420, null);

            var actual = await this.context.Quests.FirstOrDefaultAsync();
            
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateQuestShouldReturnQuest()
        {
            var expected = new Quest
            {
                Id = 1,
                Name = "test",
                Coins = 420,
            };

            var actual = await this.questService.CreateQuestAsync("test", 420, null);

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetQuestShouldReturnQuest()
        {
            var expected = new Quest
            {
                Id = 1,
                Name = "test",
                Coins = 420,
            };

            await this.questService.CreateQuestAsync("test", 420, null);

            var actual = await this.questService.GetQuestAsync("test");

            Assert.Equal(expected.ToString(), actual.ToString());

        }

        [Fact]
        public async Task GetQuestShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.questService.GetQuestAsync("test");
            });

        }

        [Fact]
        public async Task SeedQuestsShouldSeedQuests()
        {
            await this.enemyService.SeedEnemiesAsync();

            await this.questService.SeedQuestsAsync();

            Assert.True(await this.context.Quests.AnyAsync());

        }

        [Fact]
        public async Task SeedQuestsShouldSeedThreeQuests()
        {
            await this.enemyService.SeedEnemiesAsync();

            await this.questService.SeedQuestsAsync();

            Assert.Equal(3, await this.context.Quests.CountAsync());

        }
    }
}
