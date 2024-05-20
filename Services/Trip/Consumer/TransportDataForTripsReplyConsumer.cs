using MassTransit;
using Models.Transport;

namespace Trip.Consumer
{
    public class TransportDataForTripsReplyConsumer : IConsumer<GetTransportDataForTripsEventReply>
    {
        public Task Consume(ConsumeContext<GetTransportDataForTripsEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
