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
            Console.Out.WriteLine("Transport Gets Event CancelReservationTransportSyncEvent");
            return _service.CancelTransport(context.Message.OfferId);
        }
    }
}
