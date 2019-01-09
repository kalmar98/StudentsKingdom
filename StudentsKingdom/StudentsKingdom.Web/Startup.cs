using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentsKingdom.Data;
using StudentsKingdom.Data.Models;
using StudentsKingdom.Mapping;
using StudentsKingdom.Web.Middlewares;
using StudentsKingdom.Data.Services;
using StudentsKingdom.Data.Services.Contracts;
using AutoMapper;
using StudentsKingdom.Common.Constants.User;
using StudentsKingdom.Web.Controllers;
using Microsoft.AspNetCore.CookiePolicy;

namespace StudentsKingdom.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
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

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
                options.Cookie.Name = "X-CSRF-TOKEN-MOONGLADE";
                options.FormFieldName = "CSRF-TOKEN-MOONGLADE-FORM";
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Home/Index";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Home/Index";
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
            });

            //xsrf security
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper();

            AutoMapperConfig.RegisterMappings(
                this.GetType().Assembly

                );

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IStatsService, StatsService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IEnemyService, EnemyService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();


            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(5);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.UseAuthentication();

            app.UseStudentsKingdomConfiguration();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
