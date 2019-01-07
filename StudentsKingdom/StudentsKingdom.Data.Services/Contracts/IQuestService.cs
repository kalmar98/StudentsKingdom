using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IQuestService
    {
        Task SeedQuestsAsync();
        Task<Quest> CreateQuestAsync(string name, int reward, Enemy enemy);
        Task<Quest> GetQuestAsync(string name);
    }
}
