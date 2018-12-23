using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentsKingdom.Data.Services.Contracts
{
    public interface IAccountService
    {
        Task SeedAdmin();
        Task SeedRoles();
    }
}
