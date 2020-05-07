using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Session.API.Infrastructure.Repositories;
using Session.API.Model;
using Session.FunctionalTests.Base;
using StackExchange.Redis;
using Xunit;

namespace Session.FunctionalTests
{
    public class RedisSessionRepositoryTests
        : SessionScenarioBase
    {
        [Fact]
        public async Task UpdateSession_And_Add_Return_Session()
        {
            using (var server = CreateServer())
            {
                var redis = server.Host.Services.GetRequiredService<ConnectionMultiplexer>();
                var redisSessionRepository = BuildSessionRepository(redis);
                var session = await redisSessionRepository.UpdateSessionAsync(BuildSessionModel());

                Assert.NotNull(session);
            }
        }

        [Fact]
        public async Task Delete_Session_Return_Null()
        {
            using (var server = CreateServer())
            {
                var redis = server.Host.Services.GetRequiredService<ConnectionMultiplexer>();

                var redisSessionRepository = BuildSessionRepository(redis);

                var session = await redisSessionRepository.UpdateSessionAsync(BuildSessionModel());

                var deleteResult = await redisSessionRepository.DeleteSessionAsync(session.ChatId, session.AuthType);

                var result = await redisSessionRepository.GetSessionAsync(session.ChatId, session.AuthType);

                Assert.True(deleteResult);
                Assert.Null(result);
            }
        }

        private RedisSessionRepository BuildSessionRepository(ConnectionMultiplexer connMux)
        {
            return new RedisSessionRepository(connMux);
        }

        private SessionModel BuildSessionModel()
        {
            return new SessionModel
            {
                AuthType = AuthType.Telegram,
                ChatId = 1,
                ExpectedWord = 0,
                Language = "language",
                State = State.Idle,
                WordSequence = new List<int> { 1, 2, 3}
            };
        }
    }
}