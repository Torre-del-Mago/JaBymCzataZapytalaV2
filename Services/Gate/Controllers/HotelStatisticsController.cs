using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.HotelStatistics;
using Models.Hotel;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/hotelStatistics/")]
    public class HotelStatisticsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private IRequestClient<GetHotelStatisticsEvent> _requestClient { get; set; }

        public HotelStatisticsController(IPublishEndpoint publishEndpoint,IRequestClient<GetHotelStatisticsEvent> requestClient)
        {
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
        }
        
        [HttpPost("addNewWatchingClient")]
        public async Task<IActionResult> getAddNewWatchingClient([FromBody] int hotelId)
        {
            try
            {
                await _publishEndpoint.Publish(new AddWatcherEvent { HotelId = hotelId });
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost("removeWatchingClient")]
        public async Task<IActionResult> getRemoveWatchingClient([FromBody] int hotelId)
        {
            try
            {
                await _publishEndpoint.Publish(new RemoveWatcherEvent { HotelId = hotelId });
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
        
        [HttpGet("getInfo")]
        public async Task<IActionResult> getHotelStatisticsInfo([FromQuery] int hotelId)
        {
            try
            {
                var hotelStatisticsResponse =
                    await _requestClient.GetResponse<GetHotelStatisticsEventReply>(new GetHotelStatisticsEvent()
                        { HotelId = hotelId });
                var response = new HotelStatisticsInfoResponse
                {
                    HasSomebodyReservedHotel = hotelStatisticsResponse.Message.HasSomebodyReservedHotel,
                    IsSomeoneElseWatching = hotelStatisticsResponse.Message.IsSomeoneElseWatching
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }
    }
}