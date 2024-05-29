using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class RemoveOfferSyncConsumer : IConsumer<RemoveOfferSyncEvent>
    {
        private readonly IOfferService _service;
        public RemoveOfferSyncConsumer(IOfferService service)
        {
            _service = service;
        }
        public Task Consume(ConsumeContext<RemoveOfferSyncEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
