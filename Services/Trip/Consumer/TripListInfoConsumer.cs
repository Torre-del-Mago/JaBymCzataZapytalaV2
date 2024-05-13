using MassTransit;

namespace Trip.Consumer
{
    public class TripListInfoConsumer : IConsumer<TripListInfoEvent>
    {
        public async Task Consume(ConsumeContext<TripListInfoEvent> context)
        {
            /*
             Do something
             */
            await context.Publish(new TripListInfoReplyEvent() { });
        }
    }
}
