using MassTransit;
using Models.Offer;

namespace OfferCommand.Consumer
{
    public class ReserveOfferConsumer : IConsumer<ReserveOfferEvent>
    {
        public async Task Consume(ConsumeContext<ReserveOfferEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new ReserveOfferEventReply() { });
        }
    }
}
