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
    public class EnemyServiceTests
    {
        private ApplicationDbContext context;
        private IEnemyService enemyService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public EnemyServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<IEnemyService, EnemyService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.enemyService = provider.GetService<IEnemyService>();
            this.statsService = provider.GetService<IStatsService>();
        }


        [Fact]
        public async Task CreateEnemyShouldCreateEnemy()
        {
            var expected = new Enemy
            {
                Id = 1,
                Name = "Azis"
            };

            await this.enemyService.CreateEnemyAsync("Azis", null);

            var actual = await this.context.Enemies.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateEnemyShouldReturnEnemy()
        {
            var expected = new Enemy
            {
                Id = 1,
                Name = "Azis"
            };

            var actual = await this.enemyService.CreateEnemyAsync("Azis", null);

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetEnemyShouldReturnEnemy()
        {
            await this.enemyService.CreateEnemyAsync("Azis", null);

            var actual = await this.enemyService.GetEnemyAsync("Azis");

            Assert.Equal("Azis", actual.Name);

        }

        [Fact]
        public async Task GetEnemyShouldThrowExceptionWhenDatabaseIsEmpty()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await this.enemyService.GetEnemyAsync("Krisko");
            });

        }

        [Fact]
        public async Task SeedEnemiesShouldSeedItems()
        {
            await this.enemyService.SeedEnemiesAsync();

            Assert.True(await this.context.Enemies.AnyAsync());

        }

        [Fact]
        public async Task SeedEnemiesShouldSeedThreeEnemies()
        {
            await this.enemyService.SeedEnemiesAsync();

            Assert.Equal(3, await this.context.Enemies.CountAsync());

        }
    }
}
