using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class CancelReservationTransportConsumer : IConsumer<CancelReservationTransportEvent>
    {
        private readonly ITransportService _service;

        public CancelReservationTransportConsumer(ITransportService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CancelReservationTransportEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
