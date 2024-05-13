using MassTransit;

namespace Trip.Consumer
{
    public class TripInfoConsumer : IConsumer<TripInfoEvent>
    {
        public async Task Consume(ConsumeContext<TripInfoEvent> context)
        {
            /*
             Do something
             */
            await context.Publish(new TripInfoReplyEvent() { });
        }
    }
}
