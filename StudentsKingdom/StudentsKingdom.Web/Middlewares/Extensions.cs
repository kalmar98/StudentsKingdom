using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsKingdom.Web.Middlewares
{
    public static class Extensions
    {
        public static IApplicationBuilder UseStudentsKingdomConfiguration(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StudentsKingdomConfiguration>();
        }
    }
}
