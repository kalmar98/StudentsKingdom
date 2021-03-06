﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentsKingdom.Data.Models;

namespace StudentsKingdom.Data
{
    public class ApplicationDbContext : IdentityDbContext<Player>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Stats> Stats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InventoryItem>().HasKey(x => new { x.InventoryId, x.ItemId });
            base.OnModelCreating(builder);
        }
    }
}
