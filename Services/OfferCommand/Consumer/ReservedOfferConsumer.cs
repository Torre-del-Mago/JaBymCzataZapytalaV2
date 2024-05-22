using MassTransit;
using Models.Offer;

namespace OfferCommand.Consumer
{
    public class ReservedOfferConsumer : IConsumer<ReservedOfferEvent>
    {
        public Task Consume(ConsumeContext<ReservedOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
