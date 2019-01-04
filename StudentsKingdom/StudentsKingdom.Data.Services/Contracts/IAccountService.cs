using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IAccountService
    {
        Task<bool> AreUsernameOrEmailTakenAsync(string username, string email);
        
        Task<Player> CreatePlayerAsync(string username, string email);
        Task<Player> RegisterAsync(string username, string password, string email);
        Task LoginAsync(Player player, bool rememberMe);
        Task LogoutAsync();
        Task<Player> GetPlayerAsync(ClaimsPrincipal claimsPrincipal);
        Task<Player> GetPlayerAsync(string username, string password);
        Task SeedAdminAsync();
        Task SeedRolesAsync();
    }
}
