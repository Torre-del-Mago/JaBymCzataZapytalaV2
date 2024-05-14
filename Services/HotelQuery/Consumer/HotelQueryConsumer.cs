using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class HotelQueryConsumer : IConsumer<GetHotelDataForTripEvent>
    {
        public async Task Consume(ConsumeContext<GetHotelDataForTripEvent> context)
        {
            /**/
            await context.Publish(new GetHotelDataForTripEventReply() { });
        }
    }
}
