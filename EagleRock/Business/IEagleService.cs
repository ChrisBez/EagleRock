using EagleRock.Models;

namespace EagleRock.Business
{
    /// <summary>
    /// Business logic for the Eagle Service
    /// </summary>
    public interface IEagleService
    {
        /// <summary>
        /// Validates and stores the Data from the bots.
        /// </summary>
        /// <param name="data">payload from the drone</param>
        /// <returns>true if successfully saved, false if something was wrong</returns>
        Task<bool> StoreDataAsync(TrafficData data);

        /// <summary>
        /// Returns latest data for every bot
        /// </summary>
        /// <returns>List of most recent data point for each of our bots</returns>
        Task<IEnumerable<TrafficData>> GetAllDataAsync();
    }
}
