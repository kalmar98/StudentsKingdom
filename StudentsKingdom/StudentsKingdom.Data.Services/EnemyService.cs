using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class EnemyService : IEnemyService
    {
        private readonly ApplicationDbContext context;
        private readonly IStatsService statsService;

        public EnemyService(ApplicationDbContext context, IStatsService statsService)
        {
            this.context = context;
            this.statsService = statsService;
        }

        public async Task<Enemy> CreateEnemyAsync(string name, Stats stats)
        {
            var enemy = new Enemy
            {
                Name = name,
                Stats = stats
            };

            await this.context.Enemies.AddAsync(enemy);
            await this.context.SaveChangesAsync();

            return enemy;
        }

        public async Task<Enemy> GetEnemyAsync(string name)
        {
            return await this.context.Enemies.SingleAsync(x => x.Name == name);
        }

        public async Task SeedEnemiesAsync()
        {
            if (!this.context.Enemies.Any())
            {
                var easyStat = EnemyConstants.DefaultEnemyStat * EnemyConstants.EasyDifficultMultiplier;
                var mediumStat = EnemyConstants.DefaultEnemyStat * EnemyConstants.MediumDifficultMultiplier;
                var hardStat = EnemyConstants.DefaultEnemyStat * EnemyConstants.HardDifficultMultiplier;

                var easyEnemy = await this.CreateEnemyAsync(
                    EnemyConstants.DefaultEasyEnemyName,
                    await this.statsService.CreateStatsAsync(
                        health: easyStat,
                        damage: easyStat,
                        defence: easyStat,
                        vitality: easyStat,
                        strength: easyStat,
                        agility: easyStat,
                        intellect: easyStat
                        )
                    );

                var mediumEnemy = await this.CreateEnemyAsync(
                    EnemyConstants.DefaultMediumEnemyName,
                    await this.statsService.CreateStatsAsync(
                        health: mediumStat,
                        damage: mediumStat,
                        defence: mediumStat,
                        vitality: mediumStat,
                        strength: mediumStat,
                        agility: mediumStat,
                        intellect: mediumStat
                        )
                    );

                var hardEnemy = await this.CreateEnemyAsync(
                    EnemyConstants.DefaultHardEnemyName,
                    await this.statsService.CreateStatsAsync(
                        health: hardStat,
                        damage: hardStat,
                        defence: hardStat,
                        vitality: hardStat,
                        strength: hardStat,
                        agility: hardStat,
                        intellect: hardStat
                        )
                    );
            }
        }
    }
}
