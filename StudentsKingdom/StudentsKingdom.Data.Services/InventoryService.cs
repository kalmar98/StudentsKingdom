using AutoMapper;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Constants.Item;
using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext context;
        private readonly IInventoryItemService inventoryItemService;
        private readonly IItemService itemService;

        public InventoryService(ApplicationDbContext context, IInventoryItemService inventoryItemService, IItemService itemService)
        {
            this.context = context;
            this.inventoryItemService = inventoryItemService;
            this.itemService = itemService;
        }

        public async Task<Inventory> CreateInventoryAsync()
        {
            var inventory = new Inventory
            {
                Capacity = CharacterConstants.InventoryCapacity
            };

            await this.context.Inventories.AddAsync(inventory);
            await this.context.SaveChangesAsync();

            return inventory;
        }

        public async Task<Inventory> CreateInventoryAsync(LocationType locationType)
        {
            if (locationType == LocationType.Blacksmith || locationType == LocationType.Canteen)
            {
                var inventory = new Inventory
                {
                    Capacity = CharacterConstants.InventoryCapacity
                };

                await this.context.Inventories.AddAsync(inventory);
                await this.context.SaveChangesAsync();

                if (locationType == LocationType.Blacksmith)
                {
                    //малко дървено, но за сега :)

                    inventory.InventoryItems.Add(
                        await this.inventoryItemService.CreateInventoryItemAsync(
                            inventory,
                            await this.itemService.GetItemAsync(ItemConstants.DefaultSwordName)
                        ));
                    inventory.InventoryItems.Add(
                        await this.inventoryItemService.CreateInventoryItemAsync(
                            inventory,
                            await this.itemService.GetItemAsync(ItemConstants.DefaultArmourName)
                        ));


                }
                else if (locationType == LocationType.Canteen)
                {
                    inventory.InventoryItems.Add(
                         await this.inventoryItemService.CreateInventoryItemAsync(
                             inventory,
                             await this.itemService.GetItemAsync(ItemConstants.DefaultFoodName)
                         ));
                }


                await this.context.SaveChangesAsync();

                return inventory;
            }

            return null;
        }


        public async Task<bool> IsInventoryFullAsync(Inventory inventory)
        {
            return await Task.Run(() =>
            {
                return inventory.InventoryItems.Where(x=>!x.IsEquipped).Count() == inventory.Capacity;
            });

        }


    }
}
