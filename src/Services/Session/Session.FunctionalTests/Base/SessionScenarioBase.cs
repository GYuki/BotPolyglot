using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Session.FunctionalTests.Base
{
    public class SessionScenarioBase
    {
        private const string ApiUrlBase = "api/session";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(SessionScenarioBase))
               .Location;

            var host = Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder => {
                    webHostBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<SessionTestStartup>();
                });

            return host.Build().GetTestServer();
        }
    }
}