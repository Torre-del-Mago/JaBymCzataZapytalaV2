using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class ReserveTransportConsumer : IConsumer<ReserveTransportEvent>
    {
        private readonly ITransportService _service;

        public ReserveTransportConsumer(ITransportService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveTransportEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
