using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using StackExchange.Redis;

namespace Receiver.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateRedisDatabase(this IHost host, Action<ConnectionMultiplexer, IServiceProvider> seeder)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                var logger = services.GetRequiredService<ILogger<ConnectionMultiplexer>>();

                var redis = services.GetService<ConnectionMultiplexer>();

                try
                {
                    logger.LogInformation("Migrating redis associated with multiplexer {RedisName}", typeof(ConnectionMultiplexer).Name);

                    var retry = Policy.Handle<RedisException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                        });
                    
                    retry.Execute(() => InvokeRedisSeeder(seeder, redis, services));

                    logger.LogInformation("Migrated database associated with context {RedisName}", typeof(ConnectionMultiplexer).Name);
                }
                catch(System.Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the redis used on multiplexer {RedisName}", typeof(ConnectionMultiplexer).Name);
                }
            }

            return host;
        }

        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                        });
                    
                    retry.Execute(() => InvokeSeeder(seeder, context, services));

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

        private static void InvokeRedisSeeder(Action<ConnectionMultiplexer, IServiceProvider> seeder, ConnectionMultiplexer redis, IServiceProvider services)
        {
            seeder(redis, services);
        }
    }
}