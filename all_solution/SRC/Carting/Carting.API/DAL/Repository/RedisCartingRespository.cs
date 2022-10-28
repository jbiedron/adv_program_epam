using CartingService.DAL.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace CartingService.DAL.Repository
{
    public class RedisCartingRespository : ICartingRespository
    {
        private readonly ILogger<RedisCartingRespository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;


        public RedisCartingRespository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis) 
        {
            // TODO: pass logger instead of logger factory
            _logger = loggerFactory.CreateLogger<RedisCartingRespository>();
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<Cart> GetAsync(string id)
        {
            var data = await _database.StringGetAsync(id);

            if (data.IsNullOrEmpty)
            {
                return await Task.FromResult<Cart>(null);
            }
            var results = JsonSerializer.Deserialize<Cart>(data, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return await Task.FromResult<Cart>(results);
        }

        public async Task<Cart> UpdateAsync(Cart updated)
        {
            var created = await _database.StringSetAsync(updated.ExternalId, JsonSerializer.Serialize(updated));

            if (!created)
            {
                _logger.LogInformation("Unable to persist cart.");
                throw new ApplicationException("Unable to persist cart.");  // TODO: custom exception class
            }

            _logger.LogInformation("Cart persisted  with success.");

            return await GetAsync(updated.ExternalId);
        }

      
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
