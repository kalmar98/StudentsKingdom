using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IInventoryItemService
    {
        Task<InventoryItem> CreateInventoryItemAsync(Inventory inventory, Item item);
    }
}
