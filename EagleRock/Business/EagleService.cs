using EagleRock.Cache;
using EagleRock.Models;

namespace EagleRock.Business
{
    /// <inheritdoc/>
    public class EagleService: IEagleService
    {
        private ILogger<EagleService> _logger;
        private ICacheService _cacheService;

        public EagleService(ILogger<EagleService> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        /// <inheritdoc/>
        public async Task<bool> StoreDataAsync(TrafficData data)
        {
            if (data is not null) //Assume this null check is some sort of business rule validation
            { 
                //TODO: send to service bus

                return await _cacheService.AddEntryAsync(data);
            }

            _logger.LogWarning("EagleService.StartDataAsync: Invalid TrafficData sent from Bot");
            return false;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TrafficData>> GetAllDataAsync()
        {
            return await _cacheService.GetCurrentDataAsync();
        }
    }
}
