using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Session.API;

namespace Session.FunctionalTests.Base
{
    public class SessionScenarioBase
    {
        private const string ApiUrlBase = "api/session";

        public TestServer CreateServer()
        {
            return new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        public static class Get
        {
            public static string GetSession(int authType, long chatId)
                => $"{ApiUrlBase}?authType={authType}&chatId={chatId}";
        }

        public static class Post
        {
            public static string Session = $"{ApiUrlBase}/";
        }
    }
}