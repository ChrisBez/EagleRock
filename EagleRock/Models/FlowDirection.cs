using System.Text.Json.Serialization;

namespace EagleRock.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FlowDirection
    {
        Inbound,
        Outbound
    }

}