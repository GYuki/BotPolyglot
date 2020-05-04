using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace Session.FunctionalTests.Base
{
    static class HttpClientExtensions
    {
        public static HttpClient CreateIdempotentClient(this TestServer server)
        {
            var client = server.CreateClient();

            client.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());

            return client;
        }
    }
}