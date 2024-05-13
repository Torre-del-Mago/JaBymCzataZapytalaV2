using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/trip/")]
    public class TripController
    {
        private IRequestClient<TripInfoEvent> _tripRequestClient { get; set; }
        private IRequestClient<TripListInfoEvent> _tripListRequestClient { get; set; }
        public TripController(IRequestClient<TripInfoEvent> tripRequestClient,
            IRequestClient<TripListInfoEvent> _tripListRequestClient) { 
        
        }

        [HttpGet("trip-info")]
        public IEnumerable getTripInfo()
        {
            var request = new TripInfoEvent()
            {

            };
            var response = await _tripRequestClient.GetResponse<TripInfoReplyEvent>(request);
            return response.Message.TripInfo;
        }

        [HttpGet("trip-list-info")]
        public IEnumerable getTripListInfo()
        {
            var request = new TripListInfoEvent()
            {

            };
            var response = await _tripRequestClient.GetResponse<TripListInfoReplyEvent>(request);
            return response.Message.TripInfo;
        }
    }
}
