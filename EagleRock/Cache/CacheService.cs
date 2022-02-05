using EagleRock.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace EagleRock.Cache
{
    /// <inheritdoc/>
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;

        private readonly ILogger _logger;


        public CacheService(IConnectionMultiplexer redis, ILogger logger)
        {
            _redis = redis;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> AddEntryAsync(TrafficData data)
        {
            try
            {
                var db = _redis.GetDatabase();

                //happy path
                if (await db.StringSetAsync($"Bot: {data.BotId}", JsonSerializer.Serialize(data)))
                {
                    return true;
                }

                //TODO: In a production app, would investigate how likely StringSetAsync would be false
                //Do we need a retry here?
                _logger.LogWarning("Warning, Redis returned false when attempting to save");
                return false;
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "CacheService.AddEntry");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<List<TrafficData>> GetCurrentDataAsync()
        {
            try
            {
                var currentData = new List<TrafficData>();
                
                //for test assuming only one Server and one DB, would imporve this configuration in prod
                var server = _redis.GetServer(_redis.GetEndPoints().First());

                // Not sure this is best practice for Redis, it appears that using Redis Hash is the right way to do this.
                foreach(var key in server.Keys(pattern: "Bot*"))
                {
                    var entry = await GetEntryAsync(key);

                    if (entry is not null)
                        currentData.Add(entry);
                }
                
                if (currentData.Count == 0)
                {
                    _logger.LogWarning($"CacheService.GetCurrentDataAsync. Returned no data");
                }

                return currentData;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CacheService.GetCurrentDataAsync");
                return new List<TrafficData>();
            }
        }


        /// <summary>
        /// retrieves and Deserialises the Traffic Data at the Key.
        /// Assumes the Value can be converted to the TrafficData Type
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns>The TrafficData entry, or null if unsuccessful</returns>
        private async Task<TrafficData?> GetEntryAsync(string key)
        {
            try
            {
                var db = _redis.GetDatabase();

                //happy path
                var entry = await db.StringGetAsync(key);

                if (entry.IsNull)
                {
                    _logger.LogWarning($"CacheService.AddEntry. Request to retrive ID: {key} returned nil");
                }

                return JsonSerializer.Deserialize<TrafficData>(entry);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CacheService.GetEntry");
                return null;
            }
        }

    }
}
