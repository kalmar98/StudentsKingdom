using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Common.Constants.Quest;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class QuestService : IQuestService
    {
        private readonly ApplicationDbContext context;
        private readonly IEnemyService enemyService;
        private readonly IMapper mapper;

        public QuestService(ApplicationDbContext context, IEnemyService enemyService, IMapper mapper)
        {
            this.context = context;
            this.enemyService = enemyService;
            this.mapper = mapper;
        }

        public async Task<Quest> CreateQuestAsync(string name, int reward, Enemy enemy)
        {
            var quest = new Quest
            {
                Name = name,
                Coins = reward,
                Enemy = enemy
            };

            await this.context.Quests.AddAsync(quest);
            await this.context.SaveChangesAsync();

            return quest;
        }

        public async Task<Quest> GetQuestAsync(string name)
        {
            return await this.context.Quests.SingleAsync(x => x.Name == name);
        }

        public async Task SeedQuestsAsync()
        {
            if (!this.context.Quests.Any())
            {
                var easyQuest = await this.CreateQuestAsync(
                    QuestConstants.EasyQuestName,
                    QuestConstants.EasyQuestReward,
                    await this.enemyService.GetEnemyAsync(EnemyConstants.DefaultEasyEnemyName)
                    );

                var mediumQuest = await this.CreateQuestAsync(
                    QuestConstants.MediumQuestName,
                    QuestConstants.MediumQuestReward,
                    await this.enemyService.GetEnemyAsync(EnemyConstants.DefaultMediumEnemyName)
                    );

                var hardQuest = await this.CreateQuestAsync(
                    QuestConstants.HardQuestName,
                    QuestConstants.HardQuestReward,
                    await this.enemyService.GetEnemyAsync(EnemyConstants.DefaultHardEnemyName)
                    );
            }
        }
    }
}
