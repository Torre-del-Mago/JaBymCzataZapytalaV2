using MassTransit;
using Models.Offer;

namespace OfferCommand.Consumer
{
    public class CreatedOfferConsumer : IConsumer<CreatedOfferEvent>
    {

        public Task Consume(ConsumeContext<CreatedOfferEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
