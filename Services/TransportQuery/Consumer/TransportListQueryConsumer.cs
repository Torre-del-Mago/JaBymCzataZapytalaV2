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
            Console.WriteLine("Transport Gets Event");
            var @event = context.Message;
            try
            {
                var result = _service.GetTransportsForCriteria(context.Message.Criteria);
                await context.RespondAsync(new GetTransportDataForTripsEventReply()
                {
                    CorrelationId = @event.CorrelationId,
                    Transports = result
                });
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new TransportDataForTripsNotFoundEvent { CorrelationId = @event.CorrelationId });
            }
            Console.WriteLine("Transport Publish Event");
        }
    }
}
