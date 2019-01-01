using StudentsKingdom.Data.Common.Enums.Locations;
using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface ILocationService
    {
        Task SeedLocationsAsync();
        Task<Location> CreateLocationAsync(string name, Inventory inventory, LocationType type);
        Task<Location> GetLocationByTypeAsync(LocationType type);
    }
}
