using System.Threading.Tasks;
using LogicBlock.Translations.Model.Texts;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LogicBlock.Translations.Infrastructure.Repositories
{
    public class RedisTranslationRepository : ITranslationsRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisTranslationRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase(2);
        }

        public async Task<Text> GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            
            var text = await _database.StringGetAsync(key);

            if (text.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Text>(text);
        }
    }
}