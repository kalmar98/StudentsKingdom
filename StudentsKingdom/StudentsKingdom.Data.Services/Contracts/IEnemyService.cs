using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IEnemyService
    {
        Task SeedEnemiesAsync();
        Task<Enemy> CreateEnemyAsync(string name, Stats stats);
        Task<Enemy> GetEnemyAsync(string name);
    }
}
