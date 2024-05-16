using MassTransit;
using Models.Hotel;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class TransportListQueryConsumer : IConsumer<GetTransportDataForTripsEvent>
    {
        private readonly ITransportService _service;

        public TransportListQueryConsumer(ITransportService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<GetTransportDataForTripsEvent> context)
        {
            var result = _service.GetTransportsForCriteria(context.Message.Criteria);
            await context.Publish(new GetTransportDataForTripsEventReply() {Transports = result});
        }
    }
}
