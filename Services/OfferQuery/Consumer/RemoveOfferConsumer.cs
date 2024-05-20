using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class RemoveOfferConsumer : IConsumer<RemoveOffer>
    {
        private readonly IOfferService _service;
        public RemoveOfferConsumer(IOfferService service)
        {
            _service = service;
        }
        public Task Consume(ConsumeContext<RemoveOffer> context)
        {
            throw new NotImplementedException();
        }
    }
}
