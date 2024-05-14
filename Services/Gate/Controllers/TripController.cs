using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Trip;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/trip/")]
    public class TripController
    {
        private IRequestClient<GenerateTripEvent> _tripRequestClient { get; set; }
        private IRequestClient<GenerateTripsEvent> _tripListRequestClient { get; set; }
        public TripController(IRequestClient<GenerateTripEvent> tripRequestClient,
            IRequestClient<GenerateTripsEvent> tripListRequestClient) { 
            _tripRequestClient = tripRequestClient;
            _tripListRequestClient = tripListRequestClient;
        
        }

        [HttpGet("trip-info")]
        public async IEnumerable getTripInfo()
        {
            var request = new GenerateTripEvent()
            {

            };
            var response = await _tripRequestClient.GetResponse<GenerateTripEventReply>(request);
            return response.Message.TripInfo;
        }

        [HttpGet("trip-list-info")]
        public async IEnumerable getTripListInfo()
        {
            var request = new GenerateTripsEvent()
            {

            };
            var response = await _tripRequestClient.GetResponse<GenerateTripsEventReply>(request);
            return response.Message.TripInfo;
        }

        /*
         !!!!! TODO: Calculate price endpoint
         */
    }
}
