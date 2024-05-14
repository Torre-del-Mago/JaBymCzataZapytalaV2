using MassTransit;
using Models.Hotel;

namespace TransportQuery.Consumer
{
    public class TransportListQueryConsumer : IConsumer<GetHotelDataForTripsEvent>
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
