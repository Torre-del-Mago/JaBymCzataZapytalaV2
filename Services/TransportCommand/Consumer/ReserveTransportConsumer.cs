using MassTransit;
using Models.Transport;

namespace TransportCommand.Consumer
{
    public class ReserveTransportConsumer : IConsumer<ReserveTransportEvent>
    {
        public Task Consume(ConsumeContext<ReserveTransportEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
