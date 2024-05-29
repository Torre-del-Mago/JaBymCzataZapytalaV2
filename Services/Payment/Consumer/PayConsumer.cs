using MassTransit;
using Models.Payment;
using Payment.Service;

namespace Payment.Consumer
{
    public class PayConsumer : IConsumer<PayEvent>
    {
        private IPaymentService _service;
        public PayConsumer(IPaymentService paymentService) { 
            _service = paymentService;
        }
        public async Task Consume(ConsumeContext<PayEvent> context)
        {
            Random rnd = new Random();
            bool hasPaymentCompleted = rnd.Next(1, 11) == 1;
            if (!hasPaymentCompleted)
            {
                await context.Publish(new PayEventReply()
                {
                    CorrelationId = context.Message.OfferCorrelationId,
                    Answer = PayEventReply.State.REJECTED});
            }
            bool isPaymentOnTime = _service.canOfferBePaidFor(context.Message.PaymentDateTime, context.Message.OfferId);
            if(!isPaymentOnTime)
            {
                await context.Publish(new PayEventReply()
                {
                    CorrelationId = context.Message.OfferCorrelationId,
                    Answer = PayEventReply.State.REJECTED
                });
            }
            await context.Publish(new PayEventReply()
            {
                CorrelationId = context.Message.OfferCorrelationId,
                Answer = PayEventReply.State.PAID
            });
            await context.Publish(new CheckPaymentEventReply()
            {
                CorrelationId = context.Message.OfferCorrelationId,
                Answer = CheckPaymentEventReply.State.PAID
            });
        }
    }
}
