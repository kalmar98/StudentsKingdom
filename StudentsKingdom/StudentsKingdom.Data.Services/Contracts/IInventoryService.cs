using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IInventoryService
    {
        Task<Inventory> CreateInventoryAsync();
        Task<Inventory> CreateInventoryAsync(LocationType locationType);
        Task<bool> IsInventoryFullAsync(Inventory inventory);
    }
}
