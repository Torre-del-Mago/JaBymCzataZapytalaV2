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
            var result = _service.GetHotelsForCriteria(context.Message.Criteria);
            await context.Publish(new GetHotelDataForTripsEventReply() {Hotels = result});
        }
    }
}
