using AutoMapper;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Item;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext context;
        private readonly IInventoryItemService inventoryItemService;
        private readonly IItemService itemService;
        private readonly IMapper mapper;

        public InventoryService(ApplicationDbContext context, IInventoryItemService inventoryItemService, IItemService itemService, IMapper mapper)
        {
            this.context = context;
            this.inventoryItemService = inventoryItemService;
            this.itemService = itemService;
            this.mapper = mapper;
        }

        public async Task<Inventory> CreateInventoryAsync(LocationType? locationType = null)
        {
            var inventory = new Inventory
            {
                Capacity = CharacterConstants.InventoryCapacity
            };

            await this.context.Inventories.AddAsync(inventory);
            await this.context.SaveChangesAsync();

            if (locationType != null)
            {
                if (locationType == LocationType.Blacksmith)
                {
                    //малко дървено, но за сега :)

                    inventory.InventoryItems.Add(
                        await this.inventoryItemService.CreateInventoryItemAsync(
                            inventory,
                            await this.itemService.GetItemByNameAsync(ItemConstants.DefaultSwordName)
                        ));
                        

                }
                else if (locationType == LocationType.Canteen)
                {
                    //under construction
                }
            }

            await this.context.SaveChangesAsync();

            return inventory;
        }

        public async Task<bool> IsInventoryFullAsync(Inventory inventory)
        {
            return await Task.Run(() =>
            {
                return inventory.InventoryItems.Count == inventory.Capacity;
            });

        }
    }
}
