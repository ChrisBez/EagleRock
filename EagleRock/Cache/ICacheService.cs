using EagleRock.Models;

namespace EagleRock.Cache
{
    /// <summary>
    /// Manages the access to the EagleRock cache layer
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Add a new Traffic data entry. Uses the BotId property as the key.
        /// Will overwrite the existing entry for the specified BotId
        /// </summary>
        /// <param name="data">Data to cache</param>
        /// <returns>true if succesfully added, false if something went wrong</returns>
        Task<bool> AddEntryAsync(TrafficData data);

        /// <summary>
        /// Returns all bot entries currently in the cache.
        /// Assumes there is only one server and one database
        /// </summary>
        /// <returns>The current data for all bots</returns>
        Task<List<TrafficData>> GetCurrentDataAsync();
    }
}
