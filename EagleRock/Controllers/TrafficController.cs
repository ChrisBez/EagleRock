using EagleRock.Models;
using Microsoft.AspNetCore.Mvc;

namespace EagleRock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {
      
        private readonly ILogger<TrafficController> _logger;

        public TrafficController(ILogger<TrafficController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Endpoint for drone to upload it current data payload
        /// </summary>
        /// <param name="data">The Payload in json</param>
        /// <returns>Empty Ok</returns>
        [HttpPost]
        public async Task<IActionResult> UploadData([FromBody] TrafficData data)
        {
            return Ok();
        }


        /// <summary>
        /// Endpoint for drone to upload it current data payload
        /// </summary>
        /// <param name="data">The Payload in json</param>
        /// <returns>Empty Ok</returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrentStats()
        {
            var allBotsData = new List<TrafficData>();


            return Ok(allBotsData);
        }
    }
}