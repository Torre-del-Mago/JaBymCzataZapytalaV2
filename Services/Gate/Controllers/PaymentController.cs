using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Payment;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/payment/")]
    public class PaymentController
    {
        private IRequestClient<PayEvent> _requestClient { get; set; }
        public PaymentController(IRequestClient<PayEvent> requestClient) { 
        _requestClient = requestClient;
        }

        [HttpGet("check")]
        public async IEnumberable CheckPayment(Info info)
        {
            var paymentEvent = new PayEvent()
            {
                /*
                 Tu idzie inicjalizacja
                 */
            };
            var response = await _requestClient.GetResponse<PayEventReply>(paymentEvent);
            return response.Message.Status;
        }
    }
}
