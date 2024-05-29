using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class TransportQueryConsumer : IConsumer<GetTransportDataForTripEvent>
    {
        private ITransportService _service {  get; set; }
        public TransportQueryConsumer(ITransportService transportService) 
        {
            _service = transportService;
        }

        public async Task Consume(ConsumeContext<GetTransportDataForTripEvent> context)
        {
            Console.WriteLine("Transport Gets Event");
            var @event = context.Message;
            var result = _service.GetTransportForCriteria(context.Message.Criteria);
            await context.Publish(new GetTransportDataForTripEventReply() {
                Id = @event.Id,
                CorrelationId = @event.CorrelationId,
                Transport = result });
            Console.WriteLine("Transport Publish Event");
        }
    }
}
