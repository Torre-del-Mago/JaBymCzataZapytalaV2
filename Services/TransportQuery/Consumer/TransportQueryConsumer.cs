using MassTransit;
using Models.Transport;

namespace TransportQuery.Consumer
{
    public class TransportQueryConsumer : IConsumer<GetTransportDataForTripEvent>
    {
        public async Task Consume(ConsumeContext<GetTransportDataForTripEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new GetTransportDataForTripEventReply() { });
        }
    }
}
