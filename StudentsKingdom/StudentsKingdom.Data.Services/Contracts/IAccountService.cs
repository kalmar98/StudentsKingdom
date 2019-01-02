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
        
        Task<StudentsKingdomUser> CreateUserAsync(string username, string email);
        Task<StudentsKingdomUser> RegisterAsync(string username, string password, string email);
        Task LoginAsync(StudentsKingdomUser user, bool rememberMe);
        Task LogoutAsync();
        Task<StudentsKingdomUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<StudentsKingdomUser> GetUserAsync(string username, string password);
        Task SeedAdminAsync();
        Task SeedRolesAsync();
    }
}
