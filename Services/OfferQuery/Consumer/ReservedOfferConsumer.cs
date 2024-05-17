using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReservedOfferConsumer : IConsumer<ReservedOfferEvent>
    {
        private readonly IOfferService _service;

        public ReservedOfferConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReservedOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
