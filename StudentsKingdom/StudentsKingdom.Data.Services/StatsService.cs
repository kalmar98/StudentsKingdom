using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext context;

        public StatsService(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task<Stats> GetStatsAsync(int id)
        {
            return await this.context.Stats.SingleAsync(x => x.Id == id);
        }
    }
}
