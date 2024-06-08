using MassTransit;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class GetTopDepartureDestinationConsumer : IConsumer<GetTopDepartureDestinationEvent>
    {
        private ITransportService _service {  get; set; }

        public GetTopDepartureDestinationConsumer(ITransportService service)
        {
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<GetTopDepartureDestinationEvent> context)
        {
            Console.Out.WriteLine("Hotel Gets Event GetTopHotelRoomTypeEvent");
            await context.RespondAsync(new GetTopDepartureDestinationEventReply()
            {
                CorrelationId = context.Message.CorrelationId
            });
        }
    }
}