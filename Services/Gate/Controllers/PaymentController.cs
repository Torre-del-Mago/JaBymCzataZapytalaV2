using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Payment.Request;
using Models.Gate.Payment.Response;
using Models.Login;
using Models.Payment;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/payment/")]
    public class PaymentController : ControllerBase
    {
        private IRequestClient<PayEvent> _requestClient { get; set; }
        public PaymentController(IRequestClient<PayEvent> requestClient) { 
        _requestClient = requestClient;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckPayment([FromBody] PayRequest request)
        {
            try {
                var clientResponse = await _requestClient.GetResponse<PayEventReply>(
                new PayEvent()
                {
                    OfferId = request.OfferId,
                    Amount = request.Amount,
                });
                var response = new PayResponse();
                response.OfferId = clientResponse.Message.OfferId;
                if (clientResponse.Message.Answer == PayEventReply.State.PAID)
                {
                    response.Answer = PayResponse.State.PAID;
                }
                if (clientResponse.Message.Answer == PayEventReply.State.REJECTED)
                {
                    response.Answer = PayResponse.State.REJECTED;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
    }
}
