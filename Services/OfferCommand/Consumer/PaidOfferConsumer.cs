using MassTransit;
using Models.Offer;

namespace OfferCommand.Consumer
{
    public class PaidOfferConsumer : IConsumer<PaidOfferEvent>
    {
        public Task Consume(ConsumeContext<PaidOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
