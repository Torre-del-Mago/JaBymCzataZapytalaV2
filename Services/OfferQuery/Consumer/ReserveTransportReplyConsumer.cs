using MassTransit;
using Models.Transport;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReserveTransportReplyConsumer : IConsumer<ReserveTransportEventReply>
    {
        private readonly IOfferService _service;

        public ReserveTransportReplyConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveTransportEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
