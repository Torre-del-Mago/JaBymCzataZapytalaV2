using MassTransit;
using Models.Transport;

namespace Trip.Consumer
{
    public class TransportDataForTripReplyConsumer : IConsumer<GetTransportDataForTripEventReply>
    {
        public Task Consume(ConsumeContext<GetTransportDataForTripEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
