using StudentsKingdom.Data.Common.Enums.Items;
using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IItemService
    {
        Task SeedItemsAsync();
        Task<Item> CreateItemAsync(string name, int price, ItemType type, Stats stats, string image);
    }
}
