using Microsoft.AspNetCore.Http;
using StudentsKingdom.Data;
using StudentsKingdom.Data.Services;
using StudentsKingdom.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Middlewares
{
    public class AdminConfiguration
    {
        private readonly RequestDelegate next;

        public AdminConfiguration(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountService accountService)
        {
            await accountService.SeedRoles();
            await accountService.SeedAdmin();
            await this.next(context);
        }

    }
}
