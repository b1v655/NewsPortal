using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hirportal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hirportal
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
            services.AddDbContext<HirportalContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<HirportalService>();
            // Dependency injection beállítása az authentikációhoz
            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<HirportalContext>() // EF használata a TravelAgencyContext entitás kontextussal
                .AddDefaultTokenProviders(); // Alapértelmezett token generátor használata (pl. SecurityStamp-hez)
            services.AddTransient<HirportalService>();
            services.Configure<IdentityOptions>(options =>
            {
                // Jelszó komplexitására vonatkozó konfiguráció
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;

                // Hibás bejelentkezés esetén az (ideiglenes) kizárásra vonatkozó konfiguráció
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Felhasználókezelésre vonatkozó konfiguráció
                ///options.User.RequireUniqueEmail = true;
            });

            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HirportalContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<HirportalService>();
            services.AddMvc();
        }*/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Authentikációs szolgáltatás használata
            app.UseAuthentication();

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            // Adatbázis inicializálása
            var dbContext = serviceProvider.GetRequiredService<HirportalContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            DbInitalizer.Initialize(dbContext, userManager, roleManager, "App_Data");
        }
       /* public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseAuthentication();

            app.UseStaticFiles();
            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            var dbContext = serviceProvider.GetRequiredService<HirportalContext>();
            //var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            //var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            DbInitalizer.Initialize(dbContext,"App_Data");
             //DbInitalizer.Initialize(dbContext, userManager, roleManager,"App_Data");

        }*/
    }
}
