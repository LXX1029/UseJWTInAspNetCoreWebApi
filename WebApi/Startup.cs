using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
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
            services.AddControllers();
            var tokenService = new JWTTokenService(Configuration);
            services.AddSingleton<ITokenService>(tokenService);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                
   
            })
                 .AddJwtBearer(opt =>   
                 {
                     opt.RequireHttpsMetadata = false;
                     opt.SaveToken = true;
                     opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = Environment.MachineName,
                         ValidAudience = Environment.MachineName,
                         IssuerSigningKey = tokenService.GetSecurityKey(),
                         ClockSkew = TimeSpan.Zero
                     };
                 })
                 ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error");
            }
            else
            {
                app.UseExceptionHandler("/api/error/index");
            }

            //app.UseExceptionHandler(handler =>
            //{
            //    handler.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        context.Response.ContentType = "application/json";
            //        ExceptionHandlerFeature exception = (ExceptionHandlerFeature)context.Features.Get<IExceptionHandlerFeature>();
            //        if (exception != null)
            //        {
            //            var sb = new StringBuilder();
            //            sb.AppendLine($"出错路径：{exception.Path}");
            //            sb.AppendLine($"异常信息：{exception.Error.Message}");
            //            await context.Response.WriteAsync(sb.ToString()).ConfigureAwait(false);
            //        }
            //    });
            //});

            //app.UseHttpsRedirection();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
