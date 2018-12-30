using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IStatsService
    {
        Task<Stats> CreateStatsAsync(int health = 0, int damage = 0, int defence = 0, int vitality = 0, int strength = 0, int agility = 0, int intellect = 0);
    }
}
