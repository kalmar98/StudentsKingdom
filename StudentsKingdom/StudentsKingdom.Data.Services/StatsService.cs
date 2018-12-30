using AutoMapper;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public StatsService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Stats> CreateStatsAsync(int health = 0, int damage = 0, int defence = 0, int vitality = 0, int strength = 0, int agility = 0, int intellect = 0)
        {
            var stats = new Stats
            {
                Health = health,
                Damage = damage,
                Defence = defence,
                Vitality = vitality,
                Strength = strength,
                Agility = agility,
                Intellect = intellect
            };

            await this.context.Stats.AddAsync(stats);
            await this.context.SaveChangesAsync();

            return stats;
        }
    }
}
