using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Data.Common.Enums.Items;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class CharacterServiceTests
    {
        private ApplicationDbContext context;
        private ICharacterService characterService;
        private IInventoryItemService inventoryItemService;
        private IInventoryService inventoryService;
        private IItemService itemService;
        private ILocationService locationService;
        private IQuestService questService;
        private IEnemyService enemyService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public CharacterServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IEnemyService, EnemyService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.characterService = provider.GetService<ICharacterService>();
            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.inventoryService = provider.GetService<IInventoryService>();
            this.itemService = provider.GetService<IItemService>();
            this.locationService = provider.GetService<ILocationService>();
            this.questService = provider.GetService<IQuestService>();
            this.enemyService = provider.GetService<IEnemyService>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreateCharacterShouldCreateCharacter()
        {
            var expected = new Character
            {
                Id = 1,
            };

            await this.characterService.CreateCharacterAsync(0, null, null);

            var actual = await this.context.Characters.FirstOrDefaultAsync();

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task CreateCharacterShouldReturnCharacter()
        {
            var expected = new Character
            {
                Id = 1,
            };

            var actual = await this.characterService.CreateCharacterAsync(0, null, null);

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetDamageValueShouldReturnDamageValue()
        {
            int strength = 10;

            var expected = strength * CharacterConstants.DamageMultiplier;

            var actual = await this.characterService.GetDamageValueAsync(strength);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetHealthValueShouldReturnHealthValue()
        {
            int vitality = 10;

            var expected = vitality * CharacterConstants.HealthMultiplier;

            var actual = await this.characterService.GetHealthValueAsync(vitality);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetDefenceValueShouldReturnDefenceValue()
        {
            var expected = CharacterConstants.StartingArmourValue;

            var actual = await this.characterService.GetDefenceValueAsync();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CanAffordThisShouldReturnTrue()
        {
            var item = new Item
            {
                Coins = 10,
            };

            int budget = 100;

            var actual = await this.characterService.CanAffordAsync(budget, item);

            Assert.True(actual);
        }

        [Fact]
        public async Task CanAffordThisShouldReturnFalse()
        {
            var item = new Item
            {
                Coins = 1000,
            };

            int budget = 100;

            var actual = await this.characterService.CanAffordAsync(budget, item);

            Assert.False(actual);
        }

        [Fact]
        public async Task BuyShouldBuyItem()
        {
            var character = new Character
            {
                Coins = 50000,
                Inventory = await this.inventoryService.CreateInventoryAsync()
            };
            var item = new Item
            {
                Id = 1,
                Coins = 1
            };

            await this.characterService.BuyAsync(character, item);

            var expected = character.Inventory.InventoryItems.FirstOrDefault().Item;

            Assert.Same(item, expected);
        }

        [Fact]
        public async Task BuyShouldReduceCharacterCoins()
        {
            var character = new Character
            {
                Coins = 50000,
                Inventory = await this.inventoryService.CreateInventoryAsync()
            };
            var item = new Item
            {
                Id = 1,
                Coins = 1
            };

            await this.characterService.BuyAsync(character, item);

            Assert.NotEqual(50000, character.Coins);
        }

        [Fact]
        public async Task EquipShouldEquipItem()
        {
            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
                Stats = await this.statsService.CreateStatsAsync(),
            };
            var item = new Item
            {
                Id = 1,
                Stats = await this.statsService.CreateStatsAsync(health: 100),
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.EquipAsync(character, item);

            var expected = character.Inventory.InventoryItems.FirstOrDefault();
            Assert.True(expected.IsEquipped);

        }

        [Fact]
        public async Task EquipShouldGiveYouStatsBonus()
        {

            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
                Stats = await this.statsService.CreateStatsAsync(),
            };
            var item = new Item
            {
                Id = 1,
                Stats = await this.statsService.CreateStatsAsync(damage: 100),
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.EquipAsync(character, item);

            Assert.Equal(100, character.Stats.Damage);

        }

        [Fact]
        public async Task EquipConsumableShouldBeRemovedAfterUse()
        {

            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
                Stats = await this.statsService.CreateStatsAsync(),
            };
            var item = new Item
            {
                Id = 1,
                Stats = await this.statsService.CreateStatsAsync(health: 100),
                Type = ItemType.Consumable
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.EquipAsync(character, item);

            Assert.False(character.Inventory.InventoryItems.Any());

        }

        [Fact]
        public async Task UnequipShouldUnequipItem()
        {
            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
                Stats = await this.statsService.CreateStatsAsync(),
            };
            var item = new Item
            {
                Id = 1,
                Stats = await this.statsService.CreateStatsAsync(damage: 100),
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.EquipAsync(character, item);

            await this.characterService.UnequipAsync(character, ItemType.Weapon.ToString());

            var expected = character.Inventory.InventoryItems.FirstOrDefault();
            Assert.False(expected.IsEquipped);

        }

        [Fact]
        public async Task UnequipShouldReduceYourStatsBonus()
        {

            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
                Stats = await this.statsService.CreateStatsAsync(),
            };
            var item = new Item
            {
                Id = 1,
                Stats = await this.statsService.CreateStatsAsync(damage: 100),
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.EquipAsync(character, item);

            await this.characterService.UnequipAsync(character, ItemType.Weapon.ToString());

            Assert.Equal(0, character.Stats.Damage);

        }

        [Fact]
        public async Task RemoveShouldRemoveItem()
        {
            var character = new Character
            {
                Inventory = await this.inventoryService.CreateInventoryAsync(),
            };
            var item = new Item
            {
                Id = 1,
            };

            await this.inventoryItemService.CreateInventoryItemAsync(character.Inventory, item);

            await this.characterService.RemoveAsync(character, item);

            Assert.False(character.Inventory.InventoryItems.Any());
        }

        [Fact]
        public async Task TrainShouldGiveYouStatsBonus()
        {
            var character = new Character
            {
                Stats = await this.statsService.CreateStatsAsync(),
            };

            await this.characterService.TrainAsync(character, nameof(Stats.Agility));

            Assert.NotEqual(0, character.Stats.Agility);
        }

        [Fact]
        public async Task QuestShouldGiveYouCoinsIfYouWin()
        {
            var character = new Character
            {
                Stats = await this.statsService.CreateStatsAsync(health: 5000, damage: 1000)
            };

            await this.enemyService.SeedEnemiesAsync();

            await this.questService.SeedQuestsAsync();

            await this.characterService.QuestAsync(character, "Easy");

            Assert.NotEqual(0, character.Coins);
        }

        [Fact]
        public async Task QuestShouldReduceYourHealthToZeroIfYouLose()
        {
            var character = new Character
            {
                Stats = await this.statsService.CreateStatsAsync(health: 5, damage: 1)
            };

            await this.enemyService.SeedEnemiesAsync();

            await this.questService.SeedQuestsAsync();

            await this.characterService.QuestAsync(character, "Easy");

            Assert.Equal(0, character.Stats.Health);
        }

        [Fact]
        public async Task PvpShouldGiveYouRewardIfYouWin()
        {
            var leftSide = await this.characterService.CreateCharacterAsync(
                100,
                await this.statsService.CreateStatsAsync(damage: 100, health: 500),
                await this.inventoryService.CreateInventoryAsync()
                );
            var rightSide = await this.characterService.CreateCharacterAsync(
                 100,
                 await this.statsService.CreateStatsAsync(damage: 1, health: 5),
                 await this.inventoryService.CreateInventoryAsync()
                 );

            await this.itemService.SeedItemsAsync();

            var winner = await this.characterService.PvpAsync(leftSide);

            Assert.True(winner.Inventory.InventoryItems.Any());
        }

        [Fact]
        public async Task PvpShouldReturnWinnerIfYouWin()
        {
            var leftSide = await this.characterService.CreateCharacterAsync(
                100,
                await this.statsService.CreateStatsAsync(damage: 100, health: 500),
                await this.inventoryService.CreateInventoryAsync()
                );
            var rightSide = await this.characterService.CreateCharacterAsync(
                 100,
                 await this.statsService.CreateStatsAsync(damage: 1, health: 5),
                 await this.inventoryService.CreateInventoryAsync()
                 );

            await this.itemService.SeedItemsAsync();

            var winner = await this.characterService.PvpAsync(leftSide);

            Assert.NotNull(winner);
        }

        [Fact]
        public async Task PvpShouldReduceYourHealthToZeroIfYouLose()
        {
            var leftSide = await this.characterService.CreateCharacterAsync(
                100,
                await this.statsService.CreateStatsAsync(damage: 100, health: 500),
                await this.inventoryService.CreateInventoryAsync()
                );
            var rightSide = await this.characterService.CreateCharacterAsync(
                 100,
                 await this.statsService.CreateStatsAsync(damage: 10000, health: 500000),
                 await this.inventoryService.CreateInventoryAsync()
                 );

            await this.itemService.SeedItemsAsync();

            var winner = await this.characterService.PvpAsync(leftSide);

            Assert.Equal(0, leftSide.Stats.Health);
        }

        [Fact]
        public async Task PvpShouldReturnNullIfYouLose()
        {
            var leftSide = await this.characterService.CreateCharacterAsync(
                100,
                await this.statsService.CreateStatsAsync(damage: 100, health: 500),
                await this.inventoryService.CreateInventoryAsync()
                );
            var rightSide = await this.characterService.CreateCharacterAsync(
                 100,
                 await this.statsService.CreateStatsAsync(damage: 10000, health: 500000),
                 await this.inventoryService.CreateInventoryAsync()
                 );

            await this.itemService.SeedItemsAsync();

            var winner = await this.characterService.PvpAsync(leftSide);

            Assert.Null(winner);
        }

        [Fact]
        public async Task GetOpponentShouldReturnRandomOpponent()
        {
            var leftSide = await this.characterService.CreateCharacterAsync(
                100,
                await this.statsService.CreateStatsAsync(damage: 100, health: 500),
                await this.inventoryService.CreateInventoryAsync()
                );
            var rightSide = await this.characterService.CreateCharacterAsync(
                 100,
                 await this.statsService.CreateStatsAsync(damage: 1, health: 5),
                 await this.inventoryService.CreateInventoryAsync()
                 );


            var opponent = await this.characterService.GetOpponentAsync(1);

            Assert.Equal(2, rightSide.Id);
        }

        [Fact]
        public async Task FightShouldReturnMessageIfYouWin()
        {
            var leftStats = await this.statsService.CreateStatsAsync(damage: 100, health: 500);
            var rightStats = await this.statsService.CreateStatsAsync(damage: 1, health: 5);

            var actual = await this.characterService.FightAsync(leftStats, rightStats);

            Assert.Equal(CharacterConstants.LeftSideWon, actual);
        }

        [Fact]
        public async Task FightShouldReturnMessageIfYouLose()
        {
            var leftStats = await this.statsService.CreateStatsAsync(damage: 100, health: 500);
            var rightStats = await this.statsService.CreateStatsAsync(damage: 10000, health: 50000);

            var actual = await this.characterService.FightAsync(leftStats, rightStats);

            Assert.Equal(CharacterConstants.RightSideWon, actual);
        }

        [Fact]
        public async Task AttackShouldReduceEnemyHealth()
        {
            var attackerDamage = 100;
            var defenderHealth = 100;

            var healthAfterAttack = await this.characterService.AttackAsync(attackerDamage, defenderHealth);

            Assert.NotEqual(defenderHealth, healthAfterAttack);
        }

        [Fact]
        public async Task GetDamageReductionShouldReturnDamageReduction()
        {
            var defenderDefence = 100;
            var expected = (int)Math.Ceiling((decimal)defenderDefence / CharacterConstants.DamageReducer);
            
            var actual = await this.characterService.GetDamageReduction(defenderDefence);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ItemAlreadyBoughtShouldReturnTrue()
        {
            var character = new Character
            {
                Coins = 50000,
                Inventory = await this.inventoryService.CreateInventoryAsync()
            };
            var item = new Item
            {
                Id = 1,
                Coins = 1
            };

            await this.characterService.BuyAsync(character, item);

            var result = await this.characterService.ItemAlreadyBoughtAsync(character, item);

            Assert.True(result);
        }

        [Fact]
        public async Task ItemAlreadyBoughtShouldReturnFalse()
        {
            var character = new Character
            {
                Coins = 50000,
                Inventory = await this.inventoryService.CreateInventoryAsync()
            };
            var item = new Item
            {
                Id = 1,
                Coins = 1
            };

            var result = await this.characterService.ItemAlreadyBoughtAsync(character, item);

            Assert.False(result);
        }
    }
}
