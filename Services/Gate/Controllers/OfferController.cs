using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/offer/")]
    public class OfferController
    {
        private IRequestClient<ReserveEvent> _requestClient { get; set; }
        public OfferController(IRequestClient<ReserveEvent> reserveClient) { 
        
        }

        [HttpGet("reserve")]
        public bool Reserve() {
            var request = new ReserveEvent() { };
            var response = await _requestClient.GetResponse<ReserveReplyEvent>(request);
            return response.Message.HasReserved;
        }
    }
}
