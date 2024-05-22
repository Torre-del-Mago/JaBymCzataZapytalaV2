using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class ReserveTransportSyncConsumer : IConsumer<ReserveTransportSyncEvent>
    {
        private readonly ITransportService _service;

        public ReserveTransportSyncConsumer(ITransportService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveTransportSyncEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
