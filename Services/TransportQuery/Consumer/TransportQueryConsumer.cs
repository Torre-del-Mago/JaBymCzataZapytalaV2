using MassTransit;
using Models.Hotel;
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
            try
            {
                var result = _service.GetTransportForCriteria(context.Message.Criteria);
                await context.RespondAsync(new GetTransportDataForTripEventReply()
                {
                    CorrelationId = @event.CorrelationId,
                    Transport = result
                });
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new TransportDataForTripNotFoundEvent { CorrelationId = @event.CorrelationId });
            }
            Console.WriteLine("Transport Publish Event");
        }
    }
}
