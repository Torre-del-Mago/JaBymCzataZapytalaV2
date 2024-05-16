using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Offer;
using Models.Trip;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/offer/")]
    public class OfferController : ControllerBase
    {
        private IRequestClient<ReserveOfferEvent> _requestClient { get; set; }
        public OfferController(IRequestClient<ReserveOfferEvent> reserveClient) { 
            _requestClient = reserveClient;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] ReserveOfferRequest request)
        {
            try
            {
                var clientResponse = await _requestClient.GetResponse<ReserveOfferEventReply>(
                    new ReserveOfferEvent() { Offer = request.Offer, Registration = request.Registration });
                var response = new ReserveOfferResponse();
                if (clientResponse.Message.Answer == ReserveOfferEventReply.State.RESERVED)
                {
                    response.Answer = ReserveOfferResponse.State.RESERVED;
                }
                else
                {
                    response.Answer = ReserveOfferResponse.State.NOT_RESERVED;
                }
                response.OfferId = clientResponse.Message.OfferId;
                response.Error = clientResponse.Message.Error;
                response.Registration = clientResponse.Message.Registration;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
