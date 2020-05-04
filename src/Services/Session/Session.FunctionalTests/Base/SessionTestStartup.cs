using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Session.API;

namespace Session.FunctionalTests.Base
{
    class SessionTestStartup : Startup
    {
        public SessionTestStartup(IHostEnvironment env) : base(env)
        {
        }
    }
}