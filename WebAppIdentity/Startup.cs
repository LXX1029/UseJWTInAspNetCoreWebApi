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
            // ʹ�����ͻ�httpClient ���httpClient����
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
                options.Lockout.AllowedForNewUsers = true;// �����û���������ֹ�û����뱻����
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
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
            services.AddRazorPages()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                });
            services.AddLogDashboard();
            // ע�� NameRequerement ������
            services.AddScoped<IAuthorizationHandler, NameRequerementHandler>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.Configure<WeatherOptions>(this.Configuration.GetSection(nameof(WeatherOptions)));
            services.AddSingleton<IConfigureOptions<WeatherOptions>, ConfigureWeatherOptions>();
        }
        /*
        public void ConfigureContainer(ServiceRegistry services)
        {
            // ʹ�����ͻ�httpClient ���httpClient����
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
                options.Lockout.AllowedForNewUsers = true;// �����û���������ֹ�û����뱻����
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
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




            #region �Զ����м��
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
            // ��װ�ɷ���
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
                // ��ʽ1
                //var endpoint = endpoints.CreateApplicationBuilder().UseMiddleware<PingPongMiddleware>().Build();
                //endpoints.Map("/ping", endpoint);
                // ��ʽ2
                //endpoints.MapPingPong("/ping");
                // ��ʽ3  ʹ��RequireAuthorization ���Ȩ��,���ʹ��ȫ����Ȩ����ʹ��AllowAnonymous����ĳ��·�ɵ�ַ�������
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("ping pong");
                }).WithDisplayName("ping end point");

                // ������body�ж�ȡ����
                endpoints.MapGet("/readfrom", async context =>
                {
                    // ������body �ж�ȡForm��ֵ
                    //var form = await context.Request.ReadFormAsync(); 
                    // ������body �ж�ȡJsonֵ
                    var json = await context.Request.ReadFromJsonAsync<Recipe>();
                    await context.Response.WriteAsJsonAsync(json);
                }).WithDisplayName("ping end point");
                endpoints.MapWeather("/weather");



                endpoints.MapRazorPages();
            });
            // �ս�·�ɣ�������Ӧ
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("something to return");
            });
        }


    }
}
