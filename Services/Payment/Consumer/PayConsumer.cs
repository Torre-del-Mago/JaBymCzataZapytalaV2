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
            Console.Out.WriteLine($"Started checking payment for offer with id {context.Message.OfferId}");
            Random rnd = new Random();
            bool hasPaymentNotCompleted = rnd.Next(1, 11) == 1;
            if (hasPaymentNotCompleted)
            {
                Console.Out.WriteLine($"Payment failed for offer with id {context.Message.OfferId}");

                await context.RespondAsync(new PayEventReply()
                {
                    CorrelationId = context.Message.CorrelationId,
                    Answer = PayEventReply.State.REJECTED});
                return;
            }
            bool isPaymentOnTime = _service.CanOfferBePaidFor(context.Message.PaymentDateTime, context.Message.OfferId);
            if(!isPaymentOnTime)
            {
                Console.Out.WriteLine($"Payment is not on time for offer with id {context.Message.OfferId}");
                await context.RespondAsync(new PayEventReply()
                {
                    CorrelationId = context.Message.CorrelationId,
                    Answer = PayEventReply.State.REJECTED
                });
            }
            else
            {
                Guid offerCorrelationId = _repository.GetPaymentForOfferId(context.Message.OfferId).CorrelationId;
                Console.Out.WriteLine($"Payment was successfully made for offer with id {context.Message.OfferId}");
                await context.RespondAsync(new PayEventReply()
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
}
