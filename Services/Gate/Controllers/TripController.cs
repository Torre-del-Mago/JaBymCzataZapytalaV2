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
        private IRequestClient<CalculatePriceEvent> _calculatePriceClient { get; set; }
        public TripController(IRequestClient<GenerateTripEvent> tripRequestClient,
            IRequestClient<GenerateTripsEvent> tripListRequestClient,
            IRequestClient<CalculatePriceEvent> calculatePriceClient) { 
            _tripRequestClient = tripRequestClient;
            _tripListRequestClient = tripListRequestClient;
            _calculatePriceClient = calculatePriceClient;
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

        [HttpGet("calculate-price")]
        public async float getTripPrice()
        {
            var request = new CalculatePriceEvent()
            {

            };
            var response = await _tripRequestClient.GetResponse<CalculatePriceEventReply>(request);
            return response.Message.TripInfo;
        }



        /*
         !!!!! TODO: Calculate price endpoint
         */
    }
}
