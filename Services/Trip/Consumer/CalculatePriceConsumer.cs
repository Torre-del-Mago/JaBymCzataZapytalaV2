using MassTransit;
using Models.Trip;

namespace Trip.Consumer
{
    public class CalculatePriceConsumer : IConsumer<CalculatePriceEvent>
    {
        public async Task Consume(ConsumeContext<CalculatePriceEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new CalculatePriceEventReply() { });
        }
    }
}
