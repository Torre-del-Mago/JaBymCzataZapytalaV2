using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class PaidOfferConsumer : IConsumer<PaidOfferEvent>
    {
        private readonly IOfferService _service;

        public PaidOfferConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<PaidOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
