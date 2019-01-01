using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface ICharacterService
    {
        
        Task<Character> CreateCharacterAsync(int coins, Stats stats,Inventory inventory);
        Task<int> GetDamageValueAsync(int strength, IList<Item> EquippedItems = null);
        Task<int> GetHealthValueAsync(int vitality, IList<Item> EquippedItems = null);
        Task<int> GetDefenceValueAsync(IList<Item> EquippedItems = null);
    }
}
