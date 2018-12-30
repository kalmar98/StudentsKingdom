using AutoMapper;
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
        private readonly IMapper mapper;

        public InventoryService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Inventory> CreateInventoryAsync()
        {
            var inventory = new Inventory();

            await this.context.Inventories.AddAsync(inventory);
            await this.context.SaveChangesAsync();

            return inventory;
        }
    }
}
