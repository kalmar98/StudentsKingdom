using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StudentsKingdom.Common.Constants.Character;
using StudentsKingdom.Common.Enums;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<Player> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICharacterService characterService;
        private readonly IStatsService statsService;
        private readonly IInventoryService inventoryService;
        private readonly IMapper mapper;

        public AccountService(SignInManager<Player> signInManager, RoleManager<IdentityRole> roleManager, ICharacterService characterService, IStatsService statsService, IInventoryService inventoryService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.characterService = characterService;
            this.statsService = statsService;
            this.inventoryService = inventoryService;
            this.mapper = mapper;
        }

        public async Task<Player> RegisterAsync(string username, string password, string email)
        {
            var player = await this.CreatePlayerAsync(username, email);

            var createUserResult = this.signInManager.UserManager.CreateAsync(player, password).Result;

            var createRoleResult = this.signInManager.UserManager.AddToRoleAsync(player, UserRoles.Player.ToString()).Result;

            if (!createUserResult.Succeeded || !createRoleResult.Succeeded)
            {
                throw new Exception("Create User/Role failed!");
            }

            return player;
        }

        public async Task LoginAsync(Player player, bool rememberMe)
        {
            await this.signInManager.SignInAsync(player, rememberMe);
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<Player> GetPlayerAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await this.signInManager.UserManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<Player> GetPlayerAsync(string username, string password)
        {
            var player = await this.signInManager.UserManager.FindByNameAsync(username);

            if (player != null)
            {
                var passwordCheck = await signInManager.CheckPasswordSignInAsync(player, password, false);

                if (passwordCheck.Succeeded)
                {
                    return player;
                }
            }

            return null;

        }

        public async Task<Player> CreatePlayerAsync(string username, string email)
        {
            var startingStatValue = CharacterConstants.StartingStatValue;

            return new Player
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

        public async Task<bool> AreUsernameOrEmailTakenAsync(string username, string email)
        {
            var playerByName = await this.signInManager.UserManager.FindByNameAsync(username);
            var playerByEmail = await this.signInManager.UserManager.FindByEmailAsync(email);


            return playerByName != null || playerByEmail != null ? true : false;
        }

        public async Task SeedAdminAsync()
        {
            var adminRoleName = UserRoles.Admin.ToString();

            if (!signInManager.UserManager.Users.Any(x => x.UserName == adminRoleName))
            {
                //тря да го оправя :)
                var admin = new Player
                {
                    UserName = adminRoleName,
                    Email = "admin@adm.in",
                    SecurityStamp = Guid.NewGuid().ToString()

                };

                signInManager.UserManager.PasswordHasher.HashPassword(admin, adminRoleName);

                await signInManager.UserManager.CreateAsync(admin, adminRoleName);

                await signInManager.UserManager.AddToRoleAsync(admin, adminRoleName);
            }

        }

        public async Task SeedRolesAsync()
        {
            var roles = Enum.GetValues(typeof(UserRoles));
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

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task ExternalLoginCallback()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            var identity = (ClaimsIdentity)info.Principal.Identity;
            var email = identity.Claims.ElementAt(1).Value;
            var username = email.Substring(0, email.IndexOf('@'));
            

            if(!this.signInManager.UserManager.Users.Any(x=>x.UserName == username))
            {
                var player = await this.CreatePlayerAsync(username, email);
                var createUserResult = await this.signInManager.UserManager.CreateAsync(player);
                var addLoginResult = await this.signInManager.UserManager.AddLoginAsync(player, info);
                var createRoleResult = this.signInManager.UserManager.AddToRoleAsync(player, UserRoles.Player.ToString()).Result;
            }

            
            var externalLoginResult = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        }


    }
}
