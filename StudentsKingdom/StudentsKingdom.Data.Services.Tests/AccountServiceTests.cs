using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Common.Constants.Enemy;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Data.Common.Enums.Items;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StudentsKingdom.Data.Services.Tests
{
    public class AccountServiceTests
    {
        private ApplicationDbContext context;
        private SignInManager<Player> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IAccountService accountService;
        private ICharacterService characterService;
        private IInventoryItemService inventoryItemService;
        private IInventoryService inventoryService;
        private IItemService itemService;
        private IQuestService questService;
        private IEnemyService enemyService;
        private IStatsService statsService;
        private IServiceProvider provider;

        public AccountServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies());

            services.AddIdentity<Player, IdentityRole>(options =>
            {
                //for now
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = UserConstants.PasswordMinLength;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Lockout.MaxFailedAccessAttempts = 3;
                //Lockout time-а нарочно е малко ;)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
            })
                //.AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<SignInManager<Player>, SignInManager<Player>>();
            services.AddScoped<RoleManager<IdentityRole>, RoleManager<IdentityRole>>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IEnemyService, EnemyService>();
            services.AddScoped<IStatsService, StatsService>();

            this.provider = services.BuildServiceProvider();

            this.context = provider.GetService<ApplicationDbContext>();

            this.signInManager = provider.GetService<SignInManager<Player>>();
            this.roleManager = provider.GetService<RoleManager<IdentityRole>>();
            this.accountService = provider.GetService<IAccountService>();
            this.characterService = provider.GetService<ICharacterService>();
            this.inventoryItemService = provider.GetService<IInventoryItemService>();
            this.inventoryService = provider.GetService<IInventoryService>();
            this.itemService = provider.GetService<IItemService>();
            this.questService = provider.GetService<IQuestService>();
            this.enemyService = provider.GetService<IEnemyService>();
            this.statsService = provider.GetService<IStatsService>();
        }

        [Fact]
        public async Task CreatePlayerShouldReturnPlayer()
        {
            var expected = new Player
            {
                UserName = "kondyo",
                Email = "azis@azis.ko"
            };
            var actual = await this.accountService.CreatePlayerAsync("kondyo", "azis@azis.ko");

            Assert.Same(expected.ToString(), actual.ToString());
        }

        [Fact]
        public async Task AreUsernameOrEmailTakenShouldReturnFalseIfThereIsNoMatch()
        {
            
            var actual = await this.accountService.AreUsernameOrEmailTakenAsync("kondyo", "azis@azis.ko");

            Assert.False(actual);
        }

        [Fact]
        public async Task SeedAdminShouldSeedAdmin()
        {
            await this.accountService.SeedRolesAsync();
            await this.accountService.SeedAdminAsync();
            var admin = await this.context.Users.FirstAsync();
            Assert.True(await signInManager.UserManager.IsInRoleAsync(admin, UserConstants.RoleAdmin));
        }

        [Fact]
        public async Task SeedRolesShouldSeedRoles()
        {
            await this.accountService.SeedRolesAsync();
            Assert.True(await this.context.Roles.AnyAsync());
        }

    }
}
