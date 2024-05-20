using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class CreatedOfferConsumer : IConsumer<CreatedOfferEvent>
    {
        private readonly IOfferService _service;

        public CreatedOfferConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CreatedOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
