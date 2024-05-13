using MassTransit;

namespace TransportQuery.Consumer
{
    public class TransportQueryConsumer : IConsumer<TransportQueryEvent>
    {
        public Task Consume(ConsumeContext<TransportQueryEvent> context)
        {
            /**/
            await context.Publish(new TransportQueryResponseEvent() { });
        }
    }
}
