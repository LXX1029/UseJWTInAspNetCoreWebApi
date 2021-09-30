using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string logOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} || {Level} || {SourceContext:l} || {Message} || {Exception} || end {NewLine}";
            Log.Logger = new LoggerConfiguration()
                //.Filter.ByExcluding(m=>m.Level == LogEventLevel.Information)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Default", LogEventLevel.Information)  // 默认App日志级别-Information
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)   // Microsoft 日志级别-Error
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
                .WriteTo.File($"{AppContext.BaseDirectory}Logs/Dotnet9.log", rollingInterval: RollingInterval.Minute, outputTemplate: logOutputTemplate).CreateLogger();
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(builder =>builder.AddConsole())
                //.ConfigureLogging(builder => builder.AddFile())

                // 使用Seq
                //.ConfigureLogging((ctx, builder) =>
                //{
                //    builder.AddSeq();
                //})

                //    .ConfigureAppConfiguration(config =>
                //    {
                //        config.AddJsonFile("appsettings.json");
                //    })
                //.ConfigureLogging((ctx, builder) =>
                //{
                //    builder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                //    builder.AddConsole();
                //    builder.AddFile(option=> {
                //        option.Periodicity = NetEscapades.Extensions.Logging.RollingFile.PeriodicityOptions.Daily;
                //        option.LogDirectory = Directory.GetCurrentDirectory()+"//logs";
                //    });
                //})

                // 使用Serilog
                .UseSerilog()
                //.UseLamar()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseIIS();
                    webBuilder.UseStartup<Startup>();
                   // webBuilder.UseUrls(new string[] { "http://*:5555", "http://*:8085" });
                });
    }
}
