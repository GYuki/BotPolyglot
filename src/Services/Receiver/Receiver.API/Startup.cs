using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using LogicBlock.Logic;
using LogicBlock.Translations.Infrastructure;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Receiver.API.Extensions;
using Receiver.API.Infrastructure.LogicController;
using Receiver.API.States;

namespace Receiver
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => {
                var ports = GetDefinedPorts(Configuration);
                options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });

                options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });

            services.AddGrpc(options => 
            {
                options.EnableDetailedErrors = true;
            });

            services.AddDbContext<TranslationContext>(opt =>
                opt.UseMySql(Configuration.GetConnectionString("connection"), mySqlOptionsAction: sqlOpt =>
                {
                    sqlOpt.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlOpt.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }));
            
            string language = Configuration["Language"].FirstCharCapitalize();

            services.AddTransient<IIdleLogic, IdleLogic>();
            services.AddTransient<ILanguageLogic, LanguageChooseLogic>();
            services.AddTransient<IModeChooseLogic, ModeChooseLogic>();
            services.AddTransient<IArcadeActionLogic, ArcadeActionLogic>();
            services.AddTransient<ITutorialActionLogic, TutorialActionLogic>();


            services.AddTransient<ILogicController, LogicController>();

            Assembly assem = typeof(ILanguageRepository).Assembly;
            Type langType = assem.GetType($"LogicBlock.Translations.Infrastructure.Repositories.{language}LanguageRepository");
            services.AddTransient(typeof(ILanguageRepository), langType);

            services.AddTransient<IArcadeLogic, ArcadeLogic>();
            services.AddTransient<ITutorialLogic, TutorialLogic>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcReceiver.ReceiverService>();
                endpoints.MapDefaultControllerRoute();
            });
        }

        private (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var port = config.GetValue("PORT", 80);
            var grpcPort = config.GetValue("GRPC_PORT", 5001);
            return (port, grpcPort);
        }
    }
}
