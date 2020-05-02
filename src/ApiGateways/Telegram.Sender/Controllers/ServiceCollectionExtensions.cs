using ApiGateways.Telegram.Sender.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateways.Telegram.Sender.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            return services;
        }
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddTransient<ITelegramService, TelegramService>();

            services.AddHttpClient<ISessionService, SessionService>();
            services.AddHttpClient<IReceiverService, ReceiverService>();

            return services;
        }
    }
}