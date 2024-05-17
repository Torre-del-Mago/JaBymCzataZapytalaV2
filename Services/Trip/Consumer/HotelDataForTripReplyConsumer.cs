using MassTransit;
using Models.Hotel;
using Models.Transport;

namespace Trip.Consumer
{
    public class HotelDataForTripReplyConsumer : IConsumer<GetHotelDataForTripEventReply>
    {
        public Task Consume(ConsumeContext<GetHotelDataForTripEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
