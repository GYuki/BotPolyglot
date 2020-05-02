using System.Threading.Tasks;
using Newtonsoft.Json;
using Session.API.Model;
using StackExchange.Redis;

namespace Session.API.Infrastructure.Repositories
{
    public class RedisSessionRepository : ISessionRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisSessionRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteSessionAsync(long chatId, AuthType auth)
        {
            return await _database.KeyDeleteAsync($"{auth.ToString()}_{chatId.ToString()}");
        }

        public async Task<SessionModel> GetSessionAsync(long chatId, AuthType auth)
        {
            var data = await _database.StringGetAsync($"{auth.ToString()}_{chatId.ToString()}");

            if (data.IsNullOrEmpty)
                return null;
            
            return JsonConvert.DeserializeObject<SessionModel>(data);
        }

        public async Task<SessionModel> UpdateSessionAsync(SessionModel session)
        {
            var created = await _database.StringSetAsync($"{session.AuthType.ToString()}_{session.ChatId.ToString()}", JsonConvert.SerializeObject(session));

            if (!created)
            {
                return null;
            }

            return await GetSessionAsync(session.ChatId, session.AuthType);
        }
    }
}