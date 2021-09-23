using LogDashboard;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Data;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                options.UseSqlite(connectionString, builder =>
                {

                });
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.AllowedForNewUsers = true;// �����û���������ֹ�û����뱻����
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //      .AddCookie(options =>
            //      {
            //          //options.LoginPath = new PathString("/Identity/Account/Login");
            //          options.Cookie.Name = "YourAppCookieName";
            //          options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
            //          options.SlidingExpiration = true;
            //          //options.AccessDeniedPath = new PathString("/Identity/Account/Denied");
            //      });

            services.AddAuthorization(config =>
            {
                // NameHas2 ���ԣ���¼���а�������2
                config.AddPolicy("NameHas2", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireAssertion(m => m.User.Identity.Name.Contains("2")); // Assertion ��ʽ
                    policy.AddRequirements(
                         new NameRequirement("D")
                        );
                });

                // IsRecipeOwner���� ��ʾRecipe�Ƿ��ɵ�ǰ�û�����
                config.AddPolicy("IsRecipeOwner", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new IsRecipeOwnerRequirement());
                });

            });
            services.AddScoped<IAuthorizationHandler, NameRequerementHandler>();
            services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();

            services.AddRazorPages();

            services.AddScoped<RecipeService>();
            services.AddLogDashboard();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseLogDashboard();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
