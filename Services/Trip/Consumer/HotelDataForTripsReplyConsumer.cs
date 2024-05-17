using MassTransit;
using Models.Hotel;

namespace Trip.Consumer
{
    public class HotelDataForTripsReplyConsumer : IConsumer<GetHotelDataForTripsEventReply>
    {
        public Task Consume(ConsumeContext<GetHotelDataForTripsEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
