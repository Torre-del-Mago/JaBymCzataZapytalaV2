using MassTransit;
using Models.Offer;

namespace OfferCommand.Consumer
{
    public class RemoveOfferConsumer : IConsumer<RemoveOfferEvent>
    {
        public Task Consume(ConsumeContext<RemoveOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
