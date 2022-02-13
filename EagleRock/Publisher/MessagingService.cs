using EagleRock.Models;
using MassTransit;

namespace EagleRock.Publisher
{
    /// <inheritdoc/>
    public class MessagingService: IMessagingService
    {
        private readonly ILogger<MessagingService> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        /// <inheritdoc/>
        public MessagingService(ILogger<MessagingService> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        /// <inheritdoc/>
        public async Task PublishTrafficDataAsync(TrafficData data)
        {
            //TODO: reformat the TrafficData Class to be an interface, as per MassTransit recommendations
            await _publishEndpoint.Publish(data);
        }
    }
}
