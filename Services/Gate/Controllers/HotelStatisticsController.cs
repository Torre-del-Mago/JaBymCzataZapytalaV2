using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/hotelStatistics/")]
    public class HotelStatisticsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        [HttpPost("addNewWatchingClient")]
        public async Task<IActionResult> getAddNewWatchingClient([FromBody] int hotelId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("removeWatchingClient")]
        public async Task<IActionResult> getRemoveWatchingClient([FromBody] int hotelId)
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("getInfo")]
        public async Task<IActionResult> getHotelStatisticsInfo([FromQuery] int hotelId)
        {
            throw new NotImplementedException();
        }
    }
}