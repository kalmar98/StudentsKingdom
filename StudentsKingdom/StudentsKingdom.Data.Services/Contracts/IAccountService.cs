using StudentsKingdom.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IAccountService
    {
        Task Login(StudentsKingdomUser user, bool rememberMe);
        Task Logout();
        StudentsKingdomUser GetUserByNameAndPassword(string username, string password);
        Task SeedAdmin();
        Task SeedRoles();
    }
}
