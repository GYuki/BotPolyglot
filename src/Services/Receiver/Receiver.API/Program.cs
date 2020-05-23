using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using LogicBlock.Translations.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Receiver.API.Extensions;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var host = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder => {
                    webHostBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>();
                })
            .Build();

            host.MigrateDbContext<TranslationContext>((context, services) =>
            {
                var env = services.GetService<IHostEnvironment>();
                var logger = services.GetService<ILogger<TranslationContextSeed>>();
                
                new TranslationContextSeed()
                    .SeedAsync(context, env.ContentRootPath, logger)
                    .Wait();
            });

            host.MigrateRedisDatabase((redis, services) =>
            {
                var env = services.GetService<IHostEnvironment>();
                var logger = services.GetService<ILogger<TextManagerSeed>>();

                new TextManagerSeed()
                    .SeedAsync(redis, env.ContentRootPath, logger)
                    .Wait();
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
