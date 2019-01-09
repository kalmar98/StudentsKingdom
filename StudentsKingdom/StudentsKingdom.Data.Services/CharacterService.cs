using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Item;
using StudentsKingdom.Data.Common.Enums.Items;
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
        private readonly IItemService itemService;
        private readonly IQuestService questService;
        private readonly IMapper mapper;

        public CharacterService(ApplicationDbContext context, IInventoryItemService inventoryItemService, IItemService itemService, IQuestService questService, IMapper mapper)
        {
            this.context = context;
            this.inventoryItemService = inventoryItemService;
            this.itemService = itemService;
            this.questService = questService;
            this.mapper = mapper;
        }

        public async Task<Character> CreateCharacterAsync(int coins, Stats stats, Inventory inventory)
        {
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

        public async Task<int> GetDamageValueAsync(int strength)
        {
            return await Task.Run(() =>
            {
                return strength * CharacterConstants.DamageMultiplier;

            });

        }

        public async Task<int> GetHealthValueAsync(int vitality)
        {
            return await Task.Run(() =>
            {
                return vitality * CharacterConstants.HealthMultiplier;

            });

        }

        public async Task<int> GetDefenceValueAsync()
        {
            return await Task.Run(() =>
            {
                return CharacterConstants.StartingArmourValue;

            });

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

            await this.context.SaveChangesAsync();
        }

        public async Task<Item> EquipAsync(Character character, Item item)
        {
            //security
            var inventoryItem = character.Inventory.InventoryItems.FirstOrDefault(x => x.ItemId == item.Id);

            if (inventoryItem != null)
            {
                if (!character.Inventory.InventoryItems.Any(x => x.IsEquipped && x.Item.Type == item.Type))
                {
                    inventoryItem.IsEquipped = true;

                    foreach (var stat in item.Stats.GetType().GetProperties().Where(x => x.Name != "LazyLoader" && x.Name != "Id"))
                    {
                        var statValue = (int)stat.GetValue(item.Stats);

                        if (statValue > 0)
                        {
                            switch (stat.Name)
                            {
                                case nameof(item.Stats.Vitality):
                                    character.Stats.Health += await this.GetHealthValueAsync(statValue);
                                    break;
                                case nameof(item.Stats.Strength):
                                    character.Stats.Damage += await this.GetDamageValueAsync(statValue);
                                    break;
                                default:
                                    break;
                            }


                            var resultValue = (int)character.Stats.GetType().GetProperty(stat.Name).GetValue(character.Stats) + statValue;
                            if (stat.Name == nameof(item.Stats.Health))
                            {
                                if (resultValue > await this.GetHealthValueAsync(character.Stats.Vitality))
                                {
                                    continue;
                                }
                            }
                            character.Stats.GetType().GetProperty(stat.Name).SetValue(character.Stats, resultValue);

                        }
                    }

                    await this.context.SaveChangesAsync();

                    if (item.Type == ItemType.Consumable)
                    {
                        this.context.InventoryItems.Remove(inventoryItem);
                        await this.context.SaveChangesAsync();
                    }
                    return item;
                }
            }



            return null;
        }

        public async Task<Item> UnequipAsync(Character character, string typeName)
        {
            var inventoryItem = character.Inventory.InventoryItems.FirstOrDefault(x => x.Item.Type.ToString() == typeName);

            if (character.Inventory.InventoryItems.Any(x => x.IsEquipped && x.Item.Id == inventoryItem.ItemId))
            {
                inventoryItem.IsEquipped = false;
                foreach (var stat in inventoryItem.Item.Stats.GetType().GetProperties().Where(x => x.Name != "LazyLoader" && x.Name != "Id"))
                {
                    var statValue = (int)stat.GetValue(inventoryItem.Item.Stats);

                    if (statValue > 0)
                    {
                        switch (stat.Name)
                        {
                            case nameof(inventoryItem.Item.Stats.Vitality):
                                character.Stats.Health -= await this.GetHealthValueAsync(statValue);
                                break;
                            case nameof(inventoryItem.Item.Stats.Strength):
                                character.Stats.Damage -= await this.GetDamageValueAsync(statValue);
                                break;
                            default:
                                break;
                        }


                        var resultValue = (int)character.Stats.GetType().GetProperty(stat.Name).GetValue(character.Stats) - statValue;
                        character.Stats.GetType().GetProperty(stat.Name).SetValue(character.Stats, resultValue);

                    }
                }

                await this.context.SaveChangesAsync();

                return inventoryItem.Item;
            }

            return null;
        }

        public async Task<bool> RemoveAsync(Character character, Item item)
        {
            //security
            var inventoryItem = character.Inventory.InventoryItems.FirstOrDefault(x => x.ItemId == item.Id);

            if (inventoryItem != null)
            {
                this.context.InventoryItems.Remove(inventoryItem);
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> TrainAsync(Character character, string statName)
        {
            //security
            if (statName == nameof(Stats.Vitality) || statName == nameof(Stats.Strength) || statName == nameof(Stats.Agility) || statName == nameof(Stats.Intellect))
            {
                if (typeof(Stats).GetProperties().Any(x => x.Name == statName))
                {
                    var property = typeof(Stats).GetProperty(statName);
                    var value = (int)property.GetValue(character.Stats) + CharacterConstants.TrainStatValue;
                    property.SetValue(character.Stats, value);
                    if (statName == nameof(Stats.Vitality))
                    {
                        character.Stats.Health = await this.GetHealthValueAsync(value);
                    }
                    else if (statName == nameof(Stats.Strength))
                    {
                        character.Stats.Damage = await this.GetDamageValueAsync(value);
                    }

                    await this.context.SaveChangesAsync();

                    return true;
                }
            }

            

            return false;
        }

        public async Task<Quest> QuestAsync(Character character, string questName)
        {
            var quest = await this.questService.GetQuestAsync(questName);
            var result = await this.FightAsync(character.Stats, quest.Enemy.Stats);

            if (result == CharacterConstants.LeftSideWon)
            {
                character.Coins += quest.Coins;
                await this.context.SaveChangesAsync();
                return quest;
            }

            return null;
        }

        public async Task<Character> PvpAsync(Character character)
        {
            var opponent = await this.GetOpponentAsync(character.Id);

            if(opponent != null)
            {
                var result = await this.FightAsync(character.Stats, opponent.Stats);

                if (result == CharacterConstants.LeftSideWon)
                {
                    if(!character.Inventory.InventoryItems.Any(x=>x.Item.Name == ItemConstants.DefaultRelicName))
                    {
                        var inventoryItem = await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, await this.itemService.GetItemAsync(ItemConstants.DefaultRelicName));
                        character.Inventory.InventoryItems.Add(inventoryItem);
                    }
                    else
                    {
                        character.Coins += CharacterConstants.PvpWinCoins;
                    }

                    await this.context.SaveChangesAsync();
                    return character;
                }
            }

            return null;
        }

        public async Task<Character> GetOpponentAsync(int attackerId)
        {
            var random = new Random();

            var allCharacters = await this.context.Characters.Where(x => x.Id != attackerId).ToArrayAsync();

            if (!allCharacters.Any())
            {
                return null;
            }

            var opponentIndex = random.Next(0, allCharacters.Count());

            var opponent = allCharacters[opponentIndex];

            return opponent;
        }

        public async Task<string> FightAsync(Stats leftSideStats, Stats rightSideStats)
        {
            var result = string.Empty;

            var leftSideHealth = leftSideStats.Health;
            var rightSideHealth = rightSideStats.Health;

            if (leftSideHealth > 0 && rightSideHealth > 0)
            {
                var leftSideDamageReduction = await this.GetDamageReduction(rightSideStats.Defence);
                var leftSideDamage = leftSideStats.Damage - leftSideDamageReduction;


                var rightSideDamageReduction = await this.GetDamageReduction(leftSideStats.Defence);
                var rightSideDamage = rightSideStats.Damage - rightSideDamageReduction;

                while (true)
                {

                    rightSideHealth = await this.AttackAsync(leftSideDamage, rightSideHealth);
                    if (rightSideHealth <= 0)
                    {
                        leftSideStats.Health = leftSideHealth;
                        result = CharacterConstants.LeftSideWon;
                        break;
                    }


                    leftSideHealth = await this.AttackAsync(rightSideDamage, leftSideHealth);
                    if (leftSideHealth <= 0)
                    {
                        leftSideStats.Health = 0;
                        result = CharacterConstants.RightSideWon;
                        break;
                    }

                }

                await this.context.SaveChangesAsync();
            }

            return result;

        }

        public async Task<int> AttackAsync(int attackerDamage, int defenderHealth)
        {
            return await Task.Run(() =>
            {
                defenderHealth -= attackerDamage;

                return defenderHealth;
            });

        }

        public async Task<int> GetDamageReduction(int defenderDefence)
        {
            return await Task.Run(() =>
            {
                return (int)Math.Ceiling((decimal)defenderDefence / CharacterConstants.DamageReducer);
            });

        }

        public async Task<bool> ItemAlreadyBoughtAsync(Character character, Item item)
        {
            return await Task.Run(() =>
            {
                return character.Inventory.InventoryItems.Any(x => x.ItemId == item.Id);
            });
        }


    }
}
