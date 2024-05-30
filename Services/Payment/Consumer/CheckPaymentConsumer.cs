using MassTransit;
using Models.Payment;
using Payment.Service;

namespace Payment.Consumer
{
    public class CheckPaymentConsumer : IConsumer<CheckPaymentEvent>
    {
        private IPaymentService _service;
        public CheckPaymentConsumer(IPaymentService paymentService) { 
            _service = paymentService;
        }
        public async Task Consume(ConsumeContext<CheckPaymentEvent> context)
        {
            _service.insertPayment(context.Message);
        }
    }
}
