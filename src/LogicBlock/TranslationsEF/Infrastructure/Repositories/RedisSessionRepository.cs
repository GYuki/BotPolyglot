using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LogicBlock.Session.Repositories
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

        public async Task<ChatSession> GetSessionAsync(int chatId)
        {
            var data = await _database.StringGetAsync(chatId.ToString());

            if (data.IsNullOrEmpty)
                return null;
            
            return JsonConvert.DeserializeObject<ChatSession>(data);
        }
    }
}