using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

//GOALS
// use dependency injection (DI)
// use Serilog
// use Settings (appsettings.json)

// The code included for the aforementioned goals would be used as a "boilerplate" for
// future projects to provide for the listed functions. Additionally, this code could
// be included in a template project to facilitate this.
//
// Nick Hahn 19Jan2025

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Serilog Setup
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            //Dependency Injection, Logging, and use of appsettings.json
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            svc.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//base configuration file loading
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)//overriding base config if in a development environment, default to production
                .AddEnvironmentVariables();//override with local environmental variables

            //reloadOnChange allows modification of the appsettings.json at runtime
            //facilitates changing of Serilog log settings (particularly logging level) at runtime
        }
    }
}