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
        Task<int> GetDamageValueAsync(int strength);
        Task<int> GetHealthValueAsync(int vitality);
        Task<int> GetDefenceValueAsync();
        Task<bool> CanAffordAsync(int budget, Item item);
        Task<bool> ItemAlreadyBoughtAsync(Character character, Item item);
        Task BuyAsync(Character character, Item item);
        Task<Item> EquipAsync(Character character, Item item);
        Task<Item> UnequipAsync(Character character, string typeName);
        Task<bool> RemoveAsync(Character character, Item item);
        Task<bool> TrainAsync(Character character, string statName);
        Task<Quest> QuestAsync(Character character, string questName);
        Task<string> FightAsync(Stats leftSideStats, Stats rightSideStats);
        Task<int> AttackAsync(int attackerDamage, int defenderHealth);
        Task<int> GetDamageReduction(int defenderDefence);
    }
}
