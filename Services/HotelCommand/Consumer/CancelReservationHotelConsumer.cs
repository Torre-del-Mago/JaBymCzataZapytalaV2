using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class CancelReservationHotelConsumer : IConsumer<CancelReservationHotelEvent>
    {
        public Task Consume(ConsumeContext<CancelReservationHotelEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
