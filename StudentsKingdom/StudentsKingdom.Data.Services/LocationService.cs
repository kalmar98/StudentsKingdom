using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext context;
        private readonly IInventoryService inventoryService;
        private readonly IMapper mapper;

        public LocationService(ApplicationDbContext context, IInventoryService inventoryService,  IMapper mapper)
        {
            this.context = context;
            this.inventoryService = inventoryService;
            this.mapper = mapper;
        }

        public async Task<Location> GetLocationByTypeAsync(LocationType type)
        {
            return await this.context.Locations.SingleAsync(x => x.Type == type);
        }

        public async Task<Location> CreateLocationAsync(string name, Inventory inventory, LocationType type)
        {
            var location = new Location
            {
                Name = name,
                Inventory = inventory,
                Type = type
            };

            await this.context.Locations.AddAsync(location);
            await this.context.SaveChangesAsync();

            return location;
        }

        public async Task SeedLocationsAsync()
        {
            var locationNames = Enum.GetNames(typeof(LocationType));

            if (!this.context.Locations.Any())
            {
                foreach (var locationName in locationNames)
                {
                    var type = Enum.Parse<LocationType>(locationName);

                    var location = await this.CreateLocationAsync(
                        locationName,
                        type == LocationType.Blacksmith || type == LocationType.Canteen ?  await this.inventoryService.CreateInventoryAsync(type) : null,
                        type
                        );
                }
            }
        }
    }
}
