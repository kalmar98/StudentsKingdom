using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IAccountService
    {
        Task<StudentsKingdomUser> CreateUserAsync(string username, string email);
        Task<StudentsKingdomUser> RegisterAsync(string username, string password, string email);
        Task LoginAsync(StudentsKingdomUser user, bool rememberMe);
        Task LogoutAsync();
        StudentsKingdomUser GetUserByNameAndPassword(string username, string password);
        Task SeedAdminAsync();
        Task SeedRolesAsync();
    }
}
