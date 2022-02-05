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
        /// Endpoint for drone to upload it current data payload
        /// </summary>
        /// <param name="data">The Payload in json</param>
        /// <returns>Empty Ok, 400 if something went wrong</returns>
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadData([FromBody] TrafficData data)
        {
            var result = await _eagleService.StoreDataAsync(data);

            //In prod BadRequest should return more details to the caller on what went wrong
            return result ? Ok(): BadRequest();
        }

        /// <summary>
        /// Endpoint for drone to upload it current data payload
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