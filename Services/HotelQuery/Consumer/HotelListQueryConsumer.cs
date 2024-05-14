using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class HotelListQueryConsumer : IConsumer<GetHotelDataForTripsEvent>
    {
        public async Task Consume(ConsumeContext<GetHotelDataForTripsEvent> context)
        {
            /*
             Do something
             */

            await context.Publish(new GetHotelDataForTripsEventReply() { });
       }
    }
}
