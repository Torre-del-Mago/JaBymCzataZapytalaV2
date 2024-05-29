using MassTransit;
using Models.Payment;
using Payment.Repository;
using Payment.Service;

namespace Payment.Consumer
{
    public class PayConsumer : IConsumer<PayEvent>
    {
        private IPaymentService _service;
        private IPaymentRepository _repository;
        public PayConsumer(IPaymentService paymentService, IPaymentRepository repository) { 
            _repository = repository;
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
                    CorrelationId = context.Message.CorrelationId,
                    Answer = PayEventReply.State.REJECTED});
            }
            bool isPaymentOnTime = _service.canOfferBePaidFor(context.Message.PaymentDateTime, context.Message.OfferId);
            if(!isPaymentOnTime)
            {
                await context.Publish(new PayEventReply()
                {
                    CorrelationId = context.Message.CorrelationId,
                    Answer = PayEventReply.State.REJECTED
                });
            }
            Guid offerCorrelationId = _repository.GetPaymentForOfferId(context.Message.OfferId).CorrelationId;
            await context.Publish(new PayEventReply()
            {
                CorrelationId = context.Message.CorrelationId,
                Answer = PayEventReply.State.PAID
            });
            await context.Publish(new CheckPaymentEventReply()
            {
                CorrelationId = offerCorrelationId,
                Answer = CheckPaymentEventReply.State.PAID
            });
        }
    }
}
