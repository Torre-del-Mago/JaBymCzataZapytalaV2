using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class CancelReservationTransportSyncConsumer : IConsumer<CancelReservationTransportSyncEvent>
    {
        private readonly ITransportService _service;

        public CancelReservationTransportSyncConsumer(ITransportService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CancelReservationTransportSyncEvent> context)
        {
            _service.CancelTransport(context.Message.OfferId);
            throw new NotImplementedException();
        }
    }
}
