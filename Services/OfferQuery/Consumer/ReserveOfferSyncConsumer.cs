using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReserveOfferSyncConsumer : IConsumer<ReserveOfferSyncEvent>
    {
        private readonly IOfferService _service;

        public ReserveOfferSyncConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveOfferSyncEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
