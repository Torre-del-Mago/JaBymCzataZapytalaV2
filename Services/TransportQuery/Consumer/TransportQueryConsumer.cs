using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class TransportQueryConsumer : IConsumer<GetTransportDataForTripEvent>
    {
        private ITransportService _service {  get; set; }
        public TransportQueryConsumer(ITransportService transportService) {
            _service = transportService;
        }

        public async Task Consume(ConsumeContext<GetTransportDataForTripEvent> context)
        {
            /*
             Do something
             */
            var result = _service.getTransportForCriteria(context.Message.Criteria);
            await context.Publish(new GetTransportDataForTripEventReply() {Transport = result });
        }
    }
}
