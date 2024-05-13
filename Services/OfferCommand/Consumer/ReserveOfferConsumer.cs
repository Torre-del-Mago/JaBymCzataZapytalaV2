using MassTransit;

namespace OfferCommand.Consumer
{
    public class ReserveOfferConsumer : IConsumer<ReserveEvent>
    {
        public async Task Consume(ConsumeContext<ReserveEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new ReserveReplyEvent() { })
        }
    }
}
