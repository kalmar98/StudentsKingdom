using AutoMapper;
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
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext context;
        private readonly IStatsService statsService;
        private readonly IMapper mapper;

        public ItemService(ApplicationDbContext context, IStatsService statsService, IMapper mapper)
        {
            this.context = context;
            this.statsService = statsService;
            this.mapper = mapper;
        }

        public async Task<Item> CreateItemAsync(string name, int price, ItemType type, Stats stats, string image)
        {
            var item = new Item
            {
                Name = name,
                Coins = price,
                Type = type,
                Stats = stats,
                Image = image
            };

            await this.context.Items.AddAsync(item);
            await this.context.SaveChangesAsync();

            return item;
        }

        public async Task SeedItemsAsync()
        {
            if(this.context.Items.Count() != ItemConstants.DefaultItemsCount)
            {
                var sword = await this.CreateItemAsync(
                ItemConstants.DefaultSwordName,
                ItemConstants.DefaultSwordPrice,
                ItemType.Weapon,
                this.statsService.CreateStatsAsync(damage: ItemConstants.DefaultSwordDamage).Result,
                ItemConstants.DefaultSwordImage
                );

            }
            
        }

    }
}
