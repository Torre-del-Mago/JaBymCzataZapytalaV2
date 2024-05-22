using MassTransit;
using Models.Transport;

namespace TransportCommand.Consumer
{
    public class CancelReservationTransportConsumer : IConsumer<CancelReservationTransportEvent>
    {


        public Task Consume(ConsumeContext<CancelReservationTransportEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
