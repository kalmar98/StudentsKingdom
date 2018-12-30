using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Enums;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<StudentsKingdomUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICharacterService characterService;
        private readonly IStatsService statsService;
        private readonly IInventoryService inventoryService;
        private readonly IMapper mapper;

        public AccountService(SignInManager<StudentsKingdomUser> signInManager, RoleManager<IdentityRole> roleManager, ICharacterService characterService,IStatsService statsService,IInventoryService inventoryService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.characterService = characterService;
            this.statsService = statsService;
            this.inventoryService = inventoryService;
            this.mapper = mapper;
        }

        public async Task<StudentsKingdomUser> RegisterAsync(string username, string password, string email)
        {
            var user = await this.CreateUserAsync(username, email);

            var createUserResult = this.signInManager.UserManager.CreateAsync(user, password).Result;

            var createRoleResult = this.signInManager.UserManager.AddToRoleAsync(user, StudentsKingdomUserRoles.User.ToString()).Result;

            if (!createUserResult.Succeeded || !createRoleResult.Succeeded)
            {
                throw new Exception("Create User/Role failed!");
            }

            return user;
        }

        public async Task LoginAsync(StudentsKingdomUser user, bool rememberMe)
        {
            await this.signInManager.SignInAsync(user, rememberMe);
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public StudentsKingdomUser GetUserByNameAndPassword(string username, string password)
        {
            return  this.signInManager.UserManager.Users.FirstOrDefault(u =>
                u.UserName == username && signInManager.CheckPasswordSignInAsync(u, password, false).Result.Succeeded);

        }

        public async Task<StudentsKingdomUser> CreateUserAsync(string username, string email)
        {
            var startingStatValue = CharacterConstants.StartingStatValue;

            return new StudentsKingdomUser
            {
                UserName = username,
                Email = email,
                Character = await this.characterService.CreateCharacterAsync(
                    CharacterConstants.StartingCoins,
                    await this.statsService.CreateStatsAsync(
                        health: await this.characterService.GetHealthValueAsync(startingStatValue),
                        damage: await this.characterService.GetDamageValueAsync(startingStatValue),
                        defence: await this.characterService.GetDefenceValueAsync(),
                        vitality: startingStatValue,
                        strength: startingStatValue,
                        agility: startingStatValue,
                        intellect: startingStatValue
                        ),
                    await this.inventoryService.CreateInventoryAsync()
                    )
            };
        }

        public async Task SeedAdminAsync()
        {
            var adminRoleName = StudentsKingdomUserRoles.Admin.ToString();

            if (!signInManager.UserManager.Users.Any(x => x.UserName == adminRoleName))
            {
                //тря да го оправя :)
                var user = new StudentsKingdomUser
                {
                    UserName = adminRoleName,
                    Email = "admin@adm.in",
                    SecurityStamp = Guid.NewGuid().ToString()
                    
                };

                signInManager.UserManager.PasswordHasher.HashPassword(user, adminRoleName);

                await signInManager.UserManager.CreateAsync(user, adminRoleName);

                await signInManager.UserManager.AddToRoleAsync(user, adminRoleName);
            }

        }

        public async Task SeedRolesAsync()
        {
            var roles = Enum.GetValues(typeof(StudentsKingdomUserRoles));
            foreach (var role in roles)
            {
                var roleName = role.ToString();

                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }

    }
}
