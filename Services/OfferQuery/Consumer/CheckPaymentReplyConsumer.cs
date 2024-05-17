using MassTransit;
using Models.Payment;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class CheckPaymentReplyConsumer : IConsumer<CheckPaymentEventReply>
    {
        private readonly IOfferService _service;

        public CheckPaymentReplyConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CheckPaymentEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
