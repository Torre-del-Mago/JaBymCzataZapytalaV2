using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/payment/")]
    public class PaymentController
    {
        private IRequestClient<CheckPaymentEvent> _requestClient { get; set; }
        public PaymentController(IRequestClient<CheckPaymentEvent> requestClient) { }

        [HttpGet("check")]
        public async IEnumberable CheckPayment(Info info)
        {
            var paymentEvent = new CheckPaymentEvent()
            {
                /*
                 Tu idzie inicjalizacja
                 */
            };
            var response = await _requestClient.GetResponse<CheckPaymentReplyEvent>(paymentEvent);
            return response.Message.Status;
        }
    }
}
