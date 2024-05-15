using MassTransit;
using Models.Hotel;
using Models.Transport;

namespace TransportQuery.Consumer
{
    public class TransportListQueryConsumer : IConsumer<GetTransportDataForTripsEvent>
    {
        public async Task Consume(ConsumeContext<GetTransportDataForTripsEvent> context)
        {


            await context.Publish(new GetTransportDataForTripsEventReply() { });
        }
    }
}
