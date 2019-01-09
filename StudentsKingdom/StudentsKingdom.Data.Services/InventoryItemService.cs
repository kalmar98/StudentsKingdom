using AutoMapper;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly ApplicationDbContext context;

        public InventoryItemService(ApplicationDbContext context)
        { 
            this.context = context;
        }

        public async Task<InventoryItem> CreateInventoryItemAsync(Inventory inventory, Item item)
        {
            var inventoryItem = new InventoryItem
            {
                Inventory = inventory,
                Item = item,
            };

            await this.context.InventoryItems.AddAsync(inventoryItem);
            await this.context.SaveChangesAsync();

            return inventoryItem;
        }


    }
}
