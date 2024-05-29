using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class HotelQueryConsumer : IConsumer<GetHotelDataForTripEvent>
    {
        private readonly IHotelService _service;
        public HotelQueryConsumer(IHotelService service)
        {
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<GetHotelDataForTripEvent> context)
        {
            Console.WriteLine("Hotel Gets Event 1");
            var @event = context.Message;
            var result = _service.GetHotelForCriteria(context.Message.Criteria);
            if (result == null)
            {
                await context.RespondAsync(new HotelDataForTripNotFoundEvent { CorrelationId = @event.CorrelationId });
            }
            else
            {
                await context.RespondAsync(new GetHotelDataForTripEventReply()
                {
                    CorrelationId = @event.CorrelationId,
                    Hotel = result
                });
            }
            
            Console.WriteLine("Hotel Publish Event");
        }
    }
}
