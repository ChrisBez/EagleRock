using EagleRock.Business;
using EagleRock.Models;
using Microsoft.AspNetCore.Mvc;

namespace EagleRock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {
      
        private readonly ILogger<TrafficController> _logger;
        private readonly IEagleService _eagleService;

        public TrafficController(ILogger<TrafficController> logger, IEagleService eagleService)
        {
            _logger = logger;
            _eagleService = eagleService;
        }

        /// <summary>
        /// Endpoint for drone to upload its current data payload
        /// </summary>
        /// <param name="data">The Payload in json</param>
        /// <returns>Empty Ok, 400 if something went wrong</returns>
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadData([FromBody] TrafficData data)
        {
            var result = await _eagleService.StoreDataAsync(data);

            //TODO: Split this Badrequest into a 400 and 500 response depending on what broke.
            // 400 = Drone submitted a bad payload, 500 = API is in a fault state (e.g. Redis is down)
            return result ? Ok(): BadRequest();
        }

        /// <summary>
        /// Endpoint that will retrieve the latest data for each drone
        /// </summary>
        /// <param name="data">The Payload in json</param>
        /// <returns>List of the saved Data</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentStats()
        {
            var allBotsData = await _eagleService.GetAllDataAsync();

            return Ok(allBotsData);
        }
    }
}