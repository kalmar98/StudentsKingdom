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
        private readonly IMapper mapper;

        public CharacterService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
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
            
            if(this.EquippedItemsNullOrEmpty(EquippedItems))
            {
                return strength * CharacterConstants.DamageMultiplier;
            }


            return 0;
        }

        public async Task<int> GetHealthValueAsync(int vitality, IList<Item> EquippedItems = null)
        {
            if (this.EquippedItemsNullOrEmpty(EquippedItems))
            {
                return vitality * CharacterConstants.HealthMultiplier;
            }

            return 0;
        }

        public async Task<int> GetDefenceValueAsync(IList<Item> EquippedItems = null)
        {
            if (this.EquippedItemsNullOrEmpty(EquippedItems))
            {
                return 0;
            }

            return 0;
        }

        private bool EquippedItemsNullOrEmpty(IList<Item> EquippedItems)
        {
            if (EquippedItems == null || !EquippedItems.Any())
            {
                return true;
            }

            return false;
        }

        
    }
}
