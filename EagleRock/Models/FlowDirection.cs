using System.Text.Json.Serialization;

namespace EagleRock.Models
{
    /// <summary>
    /// Direction of traffic flow
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FlowDirection
    {
        /// <summary>
        /// Towards the City
        /// </summary>
        Inbound,
        /// <summary>
        /// Away from the City
        /// </summary>
        Outbound
    }

}