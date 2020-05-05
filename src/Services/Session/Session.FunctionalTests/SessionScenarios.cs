using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Session.API.Model;
using Session.FunctionalTests.Base;
using Xunit;

namespace Session.FunctionalTests
{
    public class SessionScenarios
        : SessionScenarioBase
    {
        [Fact]
        public async Task Post_Session_Should_Return_Ok()
        {
            using(var server = CreateServer())
            {
                var content = new StringContent(BuildSession(), UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Session, content);
                
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_Session_Should_Return_Ok()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.GetSession(0, 123));
                
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_Session_Should_Return_Not_Found()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.GetSession(0, 2));

                Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task Get_Session_Should_Return_Bad_Request()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.GetSession(0, 0));

                Assert.Equal((int)response.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
            }
        }

        private string BuildSession()
        {
            var words = new List<int> { 1, 2, 3 };
            var session = new SessionModel
            {
                AuthType = AuthType.Telegram,
                ChatId = 3228,
                ExpectedWord = 0,
                Language = "en",
                State = State.Idle,
                WordSequence = words
            };

            return JsonConvert.SerializeObject(session);
        }
    }
}