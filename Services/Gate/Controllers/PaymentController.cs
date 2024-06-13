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
        public PaymentController(IRequestClient<PayEvent> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckPayment([FromQuery] int OfferId, [FromQuery] double Amount)
        {
            Console.Out.WriteLine("Got Request CheckPayment with offerId: " + OfferId);
            try
            {
                var clientResponse = await _requestClient.GetResponse<PayEventReply>(
                new PayEvent()
                {
                    OfferId = OfferId,
                    Amount = Amount,
                    PaymentDateTime = DateTime.Now
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

        [HttpPost("test-check")]
        public async Task<IActionResult> TestCheckPayment([FromQuery] int OfferId, [FromQuery] int Amount)
        {
            try
            {
                Random rnd = new Random();
                bool hasPaymentNotCompleted = rnd.Next(1, 11) == 1;
                var response = new PayResponse();
                response.OfferId = OfferId;
                if (!hasPaymentNotCompleted)
                {
                    response.Answer = PayResponse.State.PAID;
                }
                if (hasPaymentNotCompleted)
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
}
