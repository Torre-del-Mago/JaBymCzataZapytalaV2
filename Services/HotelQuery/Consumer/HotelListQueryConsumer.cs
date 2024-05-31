using HotelQuery.Repository.Hotel;
using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class HotelListQueryConsumer : IConsumer<GetHotelDataForTripsEvent>
    {
        private readonly IHotelService _service;

        public HotelListQueryConsumer(IHotelService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<GetHotelDataForTripsEvent> context)
        {
            Console.WriteLine("Hotel Gets Event GetHotelDataForTripsEvent");
            var @event = context.Message;
            var result = _service.GetHotelsForCriteria(context.Message.Criteria);
            if (result == null || result.Hotels.Count() == 0) {
                await context.RespondAsync(new HotelDataForTripsNotFoundEvent { CorrelationId = @event.CorrelationId });
            }
            else {
                await context.RespondAsync(new GetHotelDataForTripsEventReply()
                {
                    CorrelationId = @event.CorrelationId,
                    Hotels = result
                });
            }
            Console.WriteLine("Hotel Publish Event GetHotelDataForTripsEventReply");
        }
    }
}
