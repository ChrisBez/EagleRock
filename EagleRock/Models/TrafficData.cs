using System.ComponentModel.DataAnnotations;

namespace EagleRock.Models
{
    public class TrafficData
    {
        /// <summary>
        /// Id of the EagleBot
        /// </summary>
        /// <example>a5a01375-7755-46e1-88f7-a37d7b358dfd</example>
        [Required]
        public Guid BotId { get; set; }

        /// <summary>
        /// WGS84 Lat
        /// </summary>
        /// <example>-27.470125</example>
        [Required]
        public double Latitude { get; set; }

        /// <summary>
        /// WGS84 Long
        /// </summary>
        /// <example>153.021072</example>
        [Required]
        public double Longitude { get; set; }

        /// <summary>
        /// Name as displayed on street sign
        /// </summary>
        /// <example>SMITH ST</example>
        public string RoadName { get; set; }

        /// <summary>
        /// Inbound or Outbound
        /// </summary>
        /// <example>Inbound</example>
        public FlowDirection? TrafficFlowDirection { get; set; }

        /// <summary>
        /// Vehicles per minute
        /// </summary>
        /// <example>24.3</example>
        public double? TrafficFlowRate { get; set; }

        /// <summary>
        /// Mean speed in Km/h
        /// </summary>
        /// <example>68.7</example>
        public double? AverageVehicalSpeed { get; set; }
    }

}