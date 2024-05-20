using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReserveOfferConsumer : IConsumer<ReserveOfferEvent>
    {
        private readonly IOfferService _service;

        public ReserveOfferConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
