using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Gate.Trip.Request;
using Models.Gate.Trip.Response;
using Models.Offer;
using Models.Trip;


namespace Gate.Controllers
{
    [ApiController]
    [Route("api/trip/")]
    public class TripController : ControllerBase
    {
        private IRequestClient<GenerateTripEvent> _tripRequestClient { get; set; }
        private IRequestClient<GenerateTripsEvent> _tripListRequestClient { get; set; }
        public TripController(IRequestClient<GenerateTripEvent> tripRequestClient,
            IRequestClient<GenerateTripsEvent> tripListRequestClient) { 
            _tripRequestClient = tripRequestClient;
            _tripListRequestClient = tripListRequestClient;
        }

        [HttpGet("trip-info")]
        public async Task<IActionResult> getTripInfo([FromQuery] GenerateTripRequest request)
        {
            try
            {
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripEventReply>(
                    new GenerateTripEvent() { Criteria = request.Criteria });
                var response = new GenerateTripResponse();
                response.TripDTO = clientResponse.Message.TripDTO;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("trip-list-info")]
        public async Task<IActionResult> getTripListInfo([FromQuery] GenerateTripsReqeust request)
        {
            try
            {
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripsEventReply>(
                    new GenerateTripsEvent() { Criteria = request.Criteria });
                var response = new GenerateTripsResponse();
                response.Trips = clientResponse.Message.Trips;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
