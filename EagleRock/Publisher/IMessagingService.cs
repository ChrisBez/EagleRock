using EagleRock.Models;

namespace EagleRock.Publisher
{
    /// <summary>
    /// Contains logic for sending messages to RabbitMq
    /// </summary>
    public interface IMessagingService
    {
        /// <summary>
        /// Sends TrafficData to RabbitMQ via MassTransit
        /// </summary>
        /// <param name="data">data to send</param>
        Task PublishTrafficDataAsync(TrafficData data);

    }
}
