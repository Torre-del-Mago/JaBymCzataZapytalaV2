using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate;
using Models.Hotel;
using Models.Transport;
using Models.TravelAgency;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/admin/")]
    public class AdminController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private IRequestClient<GetTopHotelRoomTypeEvent> _hotelRequestClient { get; set; }
        private IRequestClient<GetLastTravelAgencyChangesEvent> _travelAgencyRequestClient { get; set; }
        private IRequestClient<GetTopDepartureDestinationEvent> _transportRequestClient { get; set; }

        public AdminController(IRequestClient<GetTopHotelRoomTypeEvent> hotelRequestClient,
            IRequestClient<GetTopDepartureDestinationEvent> transportRequestClient, IRequestClient<GetLastTravelAgencyChangesEvent> travelAgencyRequestClient)
        {
            _hotelRequestClient = hotelRequestClient;
            _transportRequestClient = transportRequestClient;
            _travelAgencyRequestClient = travelAgencyRequestClient;
        }

        [HttpGet("admin-info")]
        public async Task<IActionResult> getAdminInfo()
        {
            try
            {
                Console.Out.WriteLine("Got Request getAdminInfo ");
                var hotelRequest =
                    _hotelRequestClient.GetResponse<GetTopHotelRoomTypeEventReply>(new GetTopHotelRoomTypeEvent());
                var travelAgencyRequest =
                    _travelAgencyRequestClient.GetResponse<GetLastTravelAgencyChangesEventReply>(new GetLastTravelAgencyChangesEvent());
                var transportRequest =
                    _transportRequestClient.GetResponse<GetTopDepartureDestinationEventReply>(
                        new GetTopDepartureDestinationEvent());
                var hotelReply = await hotelRequest;
                var travelAgencyReply = await travelAgencyRequest;
                var transportReply = await transportRequest;
                var response = new GetAdminDataResponse()
                {
                    TopDepartureDto = transportReply.Message.TopDepartureDto,
                    TopDestinationDto = transportReply.Message.TopDestinationDto,
                    TopHotelsDto = hotelReply.Message.TopHotelsDto,
                    TopRoomTypesDto = hotelReply.Message.TopRoomTypesDto,
                    LastTravelAgencyChangesDto = travelAgencyReply.Message.LastTravelAgencyChangesDto
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