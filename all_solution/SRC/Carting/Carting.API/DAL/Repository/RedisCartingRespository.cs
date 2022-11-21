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

        public async Task<Cart> UpdateAsync(Cart toUpdate)
        {
            var created = await _database.StringSetAsync(toUpdate.ExternalId, JsonSerializer.Serialize(toUpdate));

            if (!created)
            {
                _logger.LogInformation("Unable to persist cart.");
                throw new ApplicationException("Unable to persist cart.");  // TODO: custom exception class
            }

            _logger.LogInformation("Cart persisted  with success.");

            return await GetAsync(toUpdate.ExternalId);
        }

      
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async void UpdateCartItemsByExternalId(CartItem cartItem) 
        {
            // TODO: check how can i invoke couple of updates in single transaction in REDIS !!!!!!!!!!!!!!!!!!
            var server = GetServer();
            var data = server.Keys();

            // this is not efficient way to query data
            var allKeys = data?.Select(k => k.ToString());
            foreach (var key in allKeys) 
            { 
                var cart = await this.GetAsync(key);
                await this.UpdateInBacketItems(cart, cartItem.ExternalId, 
                    newName: cartItem.Name,         // better to pass null explicitly
                    newPrice: cartItem.Price,
                    newDescription: cartItem.Description,
                    newImageUrl: cartItem.ImageUrl);
            }
        }

        private async Task UpdateInBacketItems(Cart cart, string externalId, string newName = null, decimal? newPrice = null, string newDescription = null, string newImageUrl = null)
        {
            var itemsToUpdate = cart?.Items?.Where(i => i.ExternalId == externalId).ToList();
            itemsToUpdate.ForEach(i =>
            {
                if (newPrice != null && i.Price != newPrice)
                    i.Price = newPrice.Value;
                if (newName != null && i.Name != newName)
                    i.Name = newName;
                if (newDescription != null && i.Description != newDescription)
                    i.Description = newDescription;
                if (newImageUrl != null && i.ImageUrl != newImageUrl)
                {
                    i.ImageUrl = newImageUrl;
                }
            });
            await this.UpdateAsync(cart);
        }
    }
}
