using Lamar;
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
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using WebAppIdentity.CustomConfigure;
using WebAppIdentity.Data;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Middleware;
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
            // 使用类型化httpClient 添加httpClient服务
            services.AddTransient<CustomMessageHandler>();
            services.AddHttpClient<CustomHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://restapi.amap.com");
            })
                //.AddTransientHttpError();
                .AddCustomPolicyHandler();
            services.AddHostedService<CustomWeatherHostedService>();



            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
            //services.AddHsts(options =>
            //{
            //    options.MaxAge = TimeSpan.FromHours(1);
            //});
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                options.UseSqlite(connectionString, builder => { });
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.AllowedForNewUsers = true;// 启用用户锁定，防止用户密码被攻击
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthorization(config =>
            {
                // NameHas2 策略，登录名中包含数字2
                config.AddPolicy("NameHas2", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireAssertion(m => m.User.Identity.Name.Contains("2")); // Assertion 方式
                    policy.AddRequirements(
                         new NameRequirement("D")
                        );
                });

                // IsRecipeOwner策略 表示Recipe是否由当前用户创建
                config.AddPolicy("IsRecipeOwner", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new IsRecipeOwnerRequirement());
                });
            });
            services.AddRazorPages()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                });
            services.AddLogDashboard();
            // 注入 NameRequerement 处理类
            services.AddScoped<IAuthorizationHandler, NameRequerementHandler>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.Configure<WeatherOptions>(this.Configuration.GetSection(nameof(WeatherOptions)));
            services.AddSingleton<IConfigureOptions<WeatherOptions>, ConfigureWeatherOptions>();
        }
        /*
        public void ConfigureContainer(ServiceRegistry services)
        {
            // 使用类型化httpClient 添加httpClient服务
            services.AddHttpClient<CustomHttpClient>(client =>
            {
                client.BaseAddress = new Uri("https://restapi.amap.com");
            });
            services.AddHsts(options =>
            {
                //options.MaxAge = TimeSpan.FromHours(1);
            });
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = this.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                options.UseSqlite(connectionString, builder => { });
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.AllowedForNewUsers = true;// 启用用户锁定，防止用户密码被攻击
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthorization(config =>
            {
                // NameHas2 策略，登录名中包含数字2
                config.AddPolicy("NameHas2", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireAssertion(m => m.User.Identity.Name.Contains("2")); // Assertion 方式
                    policy.AddRequirements(
                         new NameRequirement("D")
                        );
                });

                // IsRecipeOwner策略 表示Recipe是否由当前用户创建
                config.AddPolicy("IsRecipeOwner", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new IsRecipeOwnerRequirement());
                });
            });
            services.AddRazorPages();
            services.AddLogDashboard();
            //services.AddScoped<RecipeService>();
            services.Scan(_ =>
            {
                _.AssemblyContainingType(typeof(Startup));
                _.WithDefaultConventions();
            });
            services.Configure<WeatherOptions>(this.Configuration.GetSection(nameof(WeatherOptions)));
            services.AddSingleton<IConfigureOptions<WeatherOptions>, ConfigureWeatherOptions>();
        }
        */


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseLogDashboard();




            #region 自定义中间件
            //app.Use(async (context, next) =>
            //{
            //        context.Response.OnStarting(() =>
            //        {
            //            context.Response.Headers.Add("customheader","header");
            //            return Task.CompletedTask;
            //        });
            //        await next();
            //});
            //app.UseMiddleware<HeadersMiddleware>();
            // 封装成方法
            //app.UseCustomHeaders();
            #endregion



            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<PingPongMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                // 方式1
                //var endpoint = endpoints.CreateApplicationBuilder().UseMiddleware<PingPongMiddleware>().Build();
                //endpoints.Map("/ping", endpoint);
                // 方式2
                //endpoints.MapPingPong("/ping");
                // 方式3  使用RequireAuthorization 添加权限,如果使用全局授权，则使用AllowAnonymous运行某个路由地址允许访问
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("ping pong");
                }).WithDisplayName("ping end point");

                // 从请求body中读取数据
                endpoints.MapGet("/readfrom", async context =>
                {
                    // 从请求body 中读取Form表单值
                    //var form = await context.Request.ReadFormAsync(); 
                    // 从请求body 中读取Json值
                    var json = await context.Request.ReadFromJsonAsync<Recipe>();
                    await context.Response.WriteAsJsonAsync(json);
                }).WithDisplayName("ping end point");
                endpoints.MapWeather("/weather");



                endpoints.MapRazorPages();
            });
            // 终结路由，返回响应
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("something to return");
            });
        }


    }
}
