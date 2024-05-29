using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Gate.Trip.Request;
using Models.Gate.Trip.Response;
using Models.Hotel.DTO;
using Models.Hotel;
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
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripEventReply, TripNotFoundEvent>(
                    new GenerateTripEvent() { Criteria = request.Criteria });
                
                if (clientResponse.Is(out Response<TripNotFoundEvent> responseA))
                {
                    return BadRequest();
                }
                else if (clientResponse.Is(out Response<GenerateTripEventReply> responseB))
                {
                    var response = new GenerateTripResponse();
                    response.TripDTO = responseB.Message.TripDTO;
                    return Ok(response);
                }
                return NotFound();
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
                var clientResponse = await _tripListRequestClient.GetResponse<GenerateTripsEventReply, TripsNotFoundEvent>(
                    new GenerateTripsEvent() { Criteria = request.Criteria });
                if (clientResponse.Is(out Response<TripsNotFoundEvent> responseA))
                {
                    return BadRequest();
                }
                else if (clientResponse.Is(out Response<GenerateTripsEventReply> responseB))
                {
                    var response = new GenerateTripsResponse();
                    response.Trips = responseB.Message.Trips;
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
