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
            if (!ValidateCoordinates(data.Longitude, data.Latitude)) 
            {
                return false;
            }

            //TODO: send to service bus
            return await _cacheService.AddEntryAsync(data);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TrafficData>> GetAllDataAsync()
        {
            return await _cacheService.GetCurrentDataAsync();
        }

        /// <summary>
        /// Little example method of some sort of business logic.
        /// Will check if coordinates are in Australia
        /// </summary>
        /// <returns>true if coords in Oz, false if not</returns>
        private bool ValidateCoordinates(double longitude, double latitude)
        {
            //dummy values.. I promise I would not hard code magic numbers in real code
            var farWestLong = 112.10084152480302;
            var farEastLong = 154.991464045676;
            var farNorthLat = -8.929442621533436;
            var farSouthLat = -44.34026;

            //I think its easier to work when its not in Oz rather then when it is.. 
            if (longitude > farEastLong || longitude < farWestLong || latitude > farNorthLat || latitude < farSouthLat)
            {
                _logger.LogWarning("EagleService.ValidateCoordinates: Drone is not in Australia");
                return false;
            }

            return true;
        }
    }
}
