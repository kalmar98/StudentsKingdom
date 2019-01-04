using AutoMapper;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApplicationDbContext context;
        private readonly IInventoryItemService inventoryItemService;
        private readonly IMapper mapper;

        public CharacterService(ApplicationDbContext context,IInventoryItemService inventoryItemService, IMapper mapper)
        {
            this.context = context;
            this.inventoryItemService = inventoryItemService;
            this.mapper = mapper;
        }

        public async Task<Character> CreateCharacterAsync(int coins, Stats stats, Inventory inventory)
        {
            //maybe dto and mapping somehow
            var character = new Character
            {
                Coins = coins,
                Stats = stats,
                Inventory = inventory
            };

            await this.context.Characters.AddAsync(character);
            await this.context.SaveChangesAsync();

            return character;
        }

        public async Task<int> GetDamageValueAsync(int strength, IList<Item> EquippedItems = null)
        {
            return await Task.Run(() =>
            {
                if (this.EquippedItemsNullOrEmpty(EquippedItems))
                {
                    return strength * CharacterConstants.DamageMultiplier;
                }


                return 0;
            });
            
        }

        public async Task<int> GetHealthValueAsync(int vitality, IList<Item> EquippedItems = null)
        {
            return await Task.Run(() =>
            {
                if (this.EquippedItemsNullOrEmpty(EquippedItems))
                {
                    return vitality * CharacterConstants.HealthMultiplier;
                }

                return 0;
            });
            
        }

        public async Task<int> GetDefenceValueAsync(IList<Item> EquippedItems = null)
        {
            return await Task.Run(() =>
            {
                if (this.EquippedItemsNullOrEmpty(EquippedItems))
                {
                    return CharacterConstants.StartingArmourValue;
                }

                return 0;
            });
            
        }

        private bool EquippedItemsNullOrEmpty(IList<Item> EquippedItems)
        {
            if (EquippedItems == null || !EquippedItems.Any())
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CanAffordAsync(int budget, Item item)
        {
            return await Task.Run(() =>
            {
                return budget - item.Coins >= 0;
            });
        }

        public async Task BuyAsync(Character character, Item item)
        {
            character.Coins -= item.Coins;
            var inventoryItem = await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);
            //character.Inventory.InventoryItems.Add();
            await this.context.SaveChangesAsync();
        }
    }
}
