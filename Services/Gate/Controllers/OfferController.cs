using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Offer;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/offer/")]
    public class OfferController
    {
        private IRequestClient<ReserveOfferEvent> _requestClient { get; set; }
        public OfferController(IRequestClient<ReserveOfferEvent> reserveClient) { 
            _requestClient = reserveClient;
        }

        [HttpGet("reserve")]
        public async Task<bool> Reserve() {
            var request = new ReserveOfferEvent() { };
            var response = await _requestClient.GetResponse<ReserveOfferEventReply>(request);
            return response.Message.Answer == ReserveOfferEventReply.State.RESERVED; // To change
        }
    }
}
