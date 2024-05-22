using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReservedOfferSyncConsumer : IConsumer<ReservedOfferSyncEvent>
    {
        private readonly IOfferService _service;

        public ReservedOfferSyncConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReservedOfferSyncEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
