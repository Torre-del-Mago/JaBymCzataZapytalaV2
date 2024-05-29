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
            Console.WriteLine("Hotel Gets Event");
            var @event = context.Message;
            var result = _service.GetHotelForCriteria(context.Message.Criteria);
            await context.Publish(new GetHotelDataForTripEventReply() {
                Id = @event.Id,
                CorrelationId = @event.CorrelationId,
                Hotel = result});
            Console.WriteLine("Hotel Publish Event");
        }
    }
}
